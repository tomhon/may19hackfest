using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            mapControl.MapElementClick += MapControl_MapElementClick;
        }



        private async void MapControl_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            //throw new NotImplementedException();
            if (args.MapElements.Count > 0 && args.MapElements[0] is MapIcon)
            {
                outerGrid.RowDefinitions[1].Height = new GridLength(200);
                detailsArea.Visibility = Visibility.Visible;
                MapIcon mapIcon = (MapIcon)args.MapElements[0];
                detailsTitle.Text = "Location: " + mapIcon.Title;

                StreetsidePanorama panorama = await
                    StreetsidePanorama.FindNearbyAsync(mapIcon.Location);
                if (panorama != null && streetSide.IsStreetsideSupported)
                {
                    //streetSide.Visibility = Visibility.Visible; (KNOWN ISSUE)
                    streetSide.Width = 300;
                    streetSide.Height = 160;
                    streetSide.CustomExperience = new StreetsideExperience(panorama);

                }
                else
                {

                    streetSide.Width = 0;
                    streetSide.Height = 0;
                }
            }
        }

        private async void searchGoButton_Click(object sender, RoutedEventArgs e)
        {
            string addressToGeocode = search.Text;

            Geopoint hintPoint = mapControl.Center;

            MapLocationFinderResult result =
                await MapLocationFinder.FindLocationsAsync(
                    addressToGeocode,
                    hintPoint,
                    3);

            if (result.Status == MapLocationFinderStatus.Success)
            {
                mapControl.MapElements.Clear();

                foreach (MapLocation location in result.Locations)
                {
                    MapIcon icon = new MapIcon();
                    icon.Location = location.Point;
                    icon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                    icon.Title = location.DisplayName;
                    mapControl.MapElements.Add(icon);
                }
            }
        }

        //private void another_searchGoButton_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
