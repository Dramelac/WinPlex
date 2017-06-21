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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Winplex.DAL;
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

        private string UseGeo { get; set; }

        private string LastLocation { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            UseGeo = Database.GetDatabase().ShowSetting("UseGeo");
            LastLocation = Database.GetDatabase().ShowSetting("LastLocation");
            this.Page_Loaded();

        }

        public async void Page_Loaded()
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
            this.LoadData(data);
        }

        public void LoadData(Weather_API data)
        {
            this.MainTitle.Text = "Today";
            this.MainCity.Text = data.city.name;
            this.MainWeather.Text = data.list.ElementAt(0).weather.ElementAt(0).main;
            this.MainDesc.Text = data.list.ElementAt(0).weather.ElementAt(0).description;
            this.MainDeg.Text = Weather_Main.KelvinToCelsiusString(data.list.ElementAt(0).main.temp);

            //Weather Image
            string icon = string.Format("ms-appx:///Assets/icon/{0}.png", data.list.ElementAt(0).weather.ElementAt(0).icon);
            this.MainImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));

            //Wind arrow
            this.MainWind.Visibility = Visibility.Visible;
            this.MainWind.RenderTransform = new RotateTransform { Angle = data.list.ElementAt(0).wind.deg, CenterX = 50, CenterY = 50};
            this.MainWindSpeed.Text = "Wind speed : " + data.list.ElementAt(0).wind.speed + " m/s";
        }

    }
}
