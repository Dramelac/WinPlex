using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Winplex.models;
using Winplex.Utils;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Winplex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Geolocalisation Geolocalisation { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.InitGeoStatus();

        }

        public async void InitGeoStatus()
        {
            Geolocalisation = new Geolocalisation();
            await Geolocalisation.Init();
            var pos = await Geolocalisation.GeoPosition();
            GeolocStatus.Text = "Geolocalisation: " + Geolocalisation.Status;
            Coord.Text = string.Format("Lat: {0}, Long: {1}", 
                pos.Coordinate.Point.Position.Latitude, 
                pos.Coordinate.Point.Position.Longitude);

            Weather_API data = await OpenWeatherAPI.GetWeatherData(pos.Coordinate.Point.Position.Latitude,
                pos.Coordinate.Point.Position.Longitude);
            this.data.Text = string.Format("City {0}, Message {1}, Cnt {2}", data.city.name, data.message, data.cnt);
        }
    }
}
