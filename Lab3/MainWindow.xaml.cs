using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using Lab3.Classes;

namespace Lab3
{
    public partial class MainWindow : Window
    {
        public List<MapObject> mapObjects = new List<MapObject>();
        public List<MapObject> secondList = new List<MapObject>();
        public PointLatLng point = new PointLatLng();
        public List<PointLatLng> areapoints = new List<PointLatLng>();
        public List<PointLatLng> routepoints = new List<PointLatLng>();
        int rpointc = 0;
        int apointc = 0;
        bool creationmode = false;
        bool secondact = false;
        public MainWindow()
        {
            InitializeComponent();
            MapLoad();
        }

        private void MapLoad()
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            Map.MapProvider = GMapProviders.GoogleMap;
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            Map.Position = new PointLatLng(55.012823, 82.950359);
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
        }
        

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void Map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (creationmode == true)
            {
                if (combox.SelectedIndex == 0)
                {
                    point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                    createra.IsEnabled = true;
                }
                if (combox.SelectedIndex == 1)
                {
                    point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                    createra.IsEnabled = true;
                }
                if (combox.SelectedIndex == 2)
                {
                    point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                    createra.IsEnabled = true;
                }
                if (combox.SelectedIndex == 3)
                {
                    point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                    routepoints.Add(point);
                    rpointc += 1;
                    if (rpointc >= 2)
                    {
                        createra.IsEnabled = true;
                        clearpoints.IsEnabled = true;
                    }
                }
                if (combox.SelectedIndex == 4)
                {
                    point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                    areapoints.Add(point);
                    apointc += 1;
                    if (apointc >= 3)
                    {
                        createra.IsEnabled = true;
                        clearpoints.IsEnabled = true;
                    }
                }
            }
            else
            {
                OList.Items.Clear();
                OList.Items.Add(null);
                PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                secondList = mapObjects.OrderBy(mobject => mobject.getDistance(point)).ToList();
                foreach (MapObject obj in secondList)
                {
                    string mapObjectAndDistanceString = new StringBuilder()
                        
                        .Append(" - ")
                        .Append(obj.getTitle())
                        .Append(" - ")
                        .Append(obj.getDistance(point).ToString("0.##"))
                        .Append(" м.").ToString();
                    OList.Items.Add(mapObjectAndDistanceString);
                }
                secondact = true;
                
            }

        }

        public void placepoint(PointLatLng point)
        {
            MapObject mapObject_point = new Location_c(OName.Text ,point);
            mapObjects.Add(mapObject_point);
            Map.Markers.Add(mapObject_point.GetMarker());
            OList.Items.Add( mapObjects.Last().objectName);
            createra.IsEnabled = false;
        }
        public void placecar(PointLatLng point)
        {
            MapObject mapObject_point = new Car(OName.Text,  point);
            mapObjects.Add(mapObject_point);
            Map.Markers.Add(mapObject_point.GetMarker());
            OList.Items.Add(mapObjects.Last().objectName);
            createra.IsEnabled = false;
        }
        public void placehuman(PointLatLng point)
        {
            MapObject mapObject_point = new Human(OName.Text,  point);
            mapObjects.Add(mapObject_point);
            Map.Markers.Add(mapObject_point.GetMarker());
            OList.Items.Add(mapObjects.Last().objectName);
            createra.IsEnabled = false;
        }
        public void createroute(List <PointLatLng>points)
        {
            rpointc = 0;
            MapObject mapObject_path = new Route_c(OName.Text,  points, points[0]);
            mapObjects.Add(mapObject_path);
            Map.Markers.Add(mapObject_path.GetMarker());
            OList.Items.Add(mapObjects.Last().objectName);
            createra.IsEnabled = false;
            clearpoints.IsEnabled = false;
        }
        public void createarea(List <PointLatLng>points)
        {
            apointc = 0;
            MapObject mapObject_area = new Area(OName.Text,  points, points[0]);
            mapObjects.Add(mapObject_area);
            Map.Markers.Add(mapObject_area.GetMarker());
            OList.Items.Add(mapObjects.Last().objectName);
            createra.IsEnabled = false;
            clearpoints.IsEnabled = false;
        }

        private void OList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OList.SelectedIndex > 0 && secondact == false)
            {
                Map.Position = mapObjects[OList.SelectedIndex - 1].getFocus();
            }
            else
            {
                if(OList.SelectedIndex > 0)
                {
                    Map.Position = secondList[OList.SelectedIndex - 1].getFocus();
                }
            }
        }

        private void Createra_Click(object sender, RoutedEventArgs e)
        {
            if (OName.Text == "")
            {
                MessageBox.Show("Object name is null");
            }
            else
            {
                if (combox.SelectedIndex == 0)
                {
                    placepoint(point);
                }
                if (combox.SelectedIndex == 1)
                {
                    placecar(point);
                }
                if (combox.SelectedIndex == 2)
                {
                    placehuman(point);
                }
                if (combox.SelectedIndex == 3)
                {
                    createroute(routepoints);
                }
                if (combox.SelectedIndex == 4)
                {
                    createarea(areapoints);
                }
                OName.Text = "";

                locate.IsEnabled = true;
                objfind.IsEnabled = true;
                clearpoints.IsEnabled = false;
                routepoints = new List<PointLatLng>();
                areapoints = new List<PointLatLng>();
            }
            OList.Items.Clear();
            for (int i = 0; i < mapObjects.Count; i++)
            {
                if (i == 0)
                {
                    OList.Items.Add(null);
                    OList.Items.Add(mapObjects[i].objectName);
                }
                else
                {
                    OList.Items.Add(mapObjects[i].objectName);
                }
            }
            secondact = false;
        }

        private void Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            createra.IsEnabled = false;
            clearpoints.IsEnabled = false;
            rpointc = 0;
            apointc = 0;
            areapoints = new List<PointLatLng>();
            routepoints = new List<PointLatLng>();
        }

        private void Findrb_Checked(object sender, RoutedEventArgs e)
        {
            createrb.IsChecked = false;
            creationmode = false;
        }

        private void Createrb_Checked(object sender, RoutedEventArgs e)
        {
            findrb.IsChecked = false;
            creationmode = true; 
        }

        private void Locate_Click(object sender, RoutedEventArgs e)
        {
            OList.Items.Clear();
            if (objfind.Text == "")
            {
                for (int i = 0; i < mapObjects.Count; i++)
                {
                    if (i == 0)
                    {
                        OList.Items.Add(null);
                        OList.Items.Add( mapObjects[i].objectName);
                    }
                    else
                    {
                        OList.Items.Add( mapObjects[i].objectName);
                    }
                }
                secondact = false;
            }
            else
            {
                secondact = true;
                secondList.Clear();
                for (int i = 0; i < mapObjects.Count; i++)
                {

                    if (i == 0)
                    {
                        OList.Items.Add(null);
                        if (mapObjects[i].objectName.Contains(objfind.Text))
                        {
                            OList.Items.Add( mapObjects[i].objectName);
                            secondList.Add(mapObjects[i]);      
                        }
                    }
                    else
                    {
                        if (mapObjects[i].objectName.Contains(objfind.Text))
                        {
                            OList.Items.Add(mapObjects[i].objectName);
                            secondList.Add(mapObjects[i]);
                        }
                    }
                }
            }
        }

        private void OList_MouseLeave(object sender, MouseEventArgs e)
        {
            OList.SelectedIndex = 0;
        }

        private void Clearpoints_Click(object sender, RoutedEventArgs e)
        {
            rpointc = 0;
            apointc = 0;
            areapoints = new List<PointLatLng>();
            routepoints = new List<PointLatLng>();
            clearpoints.IsEnabled = false;
            createra.IsEnabled = false;
        }

        private void OName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    

}
