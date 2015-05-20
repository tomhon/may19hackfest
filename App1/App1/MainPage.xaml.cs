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
    }
}
