using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Assignment2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {      

        public MainWindow()
        {
            InitializeComponent();

            LoadBasemaps();

            LoadFeatureLayer();

            LoadLocations();           

            DataContext = this;
        }

       /// <summary>
       /// This loads the different kinds of basemaps to the combobox
       /// </summary>
        private void LoadBasemaps()
        {
            try
            {
                BasemapCombobox.Items.Add("World Street Map");
                BasemapCombobox.Items.Add("World Topographic Map");
                BasemapCombobox.Items.Add("World Imagery");
                BasemapCombobox.Items.Add("Night Streets BaseMap");
                BasemapCombobox.Items.Add("Open Street Map BaseMap");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// This method loads the feature layer on the map
        /// </summary>
        private void LoadFeatureLayer()
        {
            try
            {
                MyMapView.Map = new Map();
                MyMapView.Map.Basemap = Basemap.CreateStreetsNightVector();

                Uri popUri = new
                    Uri("https://services1.arcgis.com/4yjifSiIG17X0gW4/arcgis/rest/services/World_Population_Data_2015_from_UN/FeatureServer/0");
                FeatureLayer flayer = new FeatureLayer(popUri);

                MyMapView.Map.OperationalLayers.Add(flayer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// this method helps to load the 5 locations in the listbox
        /// </summary>

        private void LoadLocations()
        {
            try
            {
                GoTimeListbox.Items.Add("Canada");
                GoTimeListbox.Items.Add("Thailand");
                GoTimeListbox.Items.Add("Iceland");
                GoTimeListbox.Items.Add("India");
                GoTimeListbox.Items.Add("Rock of Gibraltar");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// this method display the value of country name region name and its population when a location is tapped on the map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MyMap_GeoViewTapped(object sender, Esri.ArcGISRuntime.UI.Controls.GeoViewInputEventArgs e)
        {
            try
            {
                //identify all layers in the MapView, passing the tap point, tolerance,
                //types to return, and max results
                IReadOnlyList<IdentifyLayerResult> idlayerResults =
                    await MyMapView.IdentifyLayersAsync(e.Position, 10, false, 5);

                if (idlayerResults.Count > 0)
                {
                    IdentifyLayerResult idResults = idlayerResults.FirstOrDefault();
                    FeatureLayer idLayer = idResults.LayerContent as FeatureLayer;
                    idLayer.ClearSelection();

                    CountryTitleLabel.Content = "Country";
                    RegionTitleLabel.Content = "Region";
                    PopulationTitleLabel.Content = "2015 Population";

                    foreach (GeoElement idElement in idResults.GeoElements)
                    {
                        Feature idFeature = idElement as Feature;
                        idLayer.SelectFeature(idFeature);

                        IDictionary<string, object> attributes = idFeature.Attributes;
                        string attKey = string.Empty;
                        object attVal1 = new object();

                        foreach (var attribute in attributes)
                        {
                            attKey = attribute.Key;
                            attVal1 = attribute.Value;

                            if (string.Compare(attKey, "Country") == 0)
                            {
                                CountryValueLabel.Content = attVal1;
                            }

                            if (string.Compare(attKey, "Major_Region") == 0)
                            {
                                RegionValueLabel.Content = attVal1;
                            }

                            if (string.Compare(attKey, "pop2015") == 0)
                            {
                                string pop = attVal1 + "000";
                                int.TryParse(pop, out int result);
                                string formatString = String.Format("{0:n0}", result);
                                PopulationValueLabel.Content = formatString;
                            }

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }     

        /// <summary>
        /// this method helps to change the view of the basemap as the selection is made from the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasemapCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = BasemapCombobox.SelectedIndex;

                switch (index)
                {
                    case 0:
                        MyMapView.Map.Basemap = Basemap.CreateStreets();
                        break;

                    case 1:
                        MyMapView.Map.Basemap = Basemap.CreateTopographic();
                        break;

                    case 2:
                        MyMapView.Map.Basemap = Basemap.CreateImagery();
                        break;

                    case 3:
                        MyMapView.Map.Basemap = Basemap.CreateStreetsNightVector();
                        break;

                    case 4:
                        MyMapView.Map.Basemap = Basemap.CreateOpenStreetMap();
                        break;

                    default:
                        MyMapView.Map.Basemap = Basemap.CreateOpenStreetMap();
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
        }

        /// <summary>
        /// this method helps to navigate through the basemap as a location in the listbox is made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GoTimeListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                double x = 1.0;
                double y = 1.0;

                int index = GoTimeListbox.SelectedIndex;

                switch (index)
                {
                    case 0: //Canada
                        x = -113.712785;
                        y = 54.6985831;
                        break;

                    case 1: //Thailand
                        x = 96.9946297;
                        y = 13.0003408;
                        break;

                    case 2: // Iceland
                        x = -23.7277777;
                        y = 64.7967723;
                        break;

                    case 3: // India
                        x = 73.7293199;
                        y = 20.7505273;
                        break;

                    case 4: // Rock of Gibraltor
                        x = -5.3504789;
                        y = 36.1440926;
                        break;

                    default: //Canada
                        x = 54.6985831;
                        y = -113.712785;
                        break;
                }

                MapPoint point = new MapPoint(x, y, SpatialReference.Create(4326));

                await MyMapView.SetViewpointCenterAsync(point);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
