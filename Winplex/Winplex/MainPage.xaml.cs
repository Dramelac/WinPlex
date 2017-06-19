using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Winplex.models;
using Winplex.Utils;


namespace Winplex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Geolocalisation Geolocalisation { get; set; }

        public ObservableCollection<WeatherDayVM> NextDays { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.NextDays = new ObservableCollection<WeatherDayVM>();
            this.Page_Loaded();
        }

        public async void Page_Loaded()
        {
            Geolocalisation = new Geolocalisation();
            await Geolocalisation.Init();
            GeolocStatus.Text = "Geolocalisation: " + Geolocalisation.Status;
            if (Geolocalisation.Status == "Enable")
            {
                var pos = await Geolocalisation.GeoPosition();
                /*Coord.Text = string.Format("Lat: {0}, Long: {1}",
                    pos.Coordinate.Point.Position.Latitude,
                    pos.Coordinate.Point.Position.Longitude);*/

                Weather_API data = await OpenWeatherAPI.GetWeatherData(pos.Coordinate.Point.Position.Latitude,
                    pos.Coordinate.Point.Position.Longitude);
                this.LoadData(data);
            }
            else
            {
                this.FailGeoloc();
            }
        }

        public void FailGeoloc()
        {
            this.MainTitle.Text = "Location is not available. Please choose a city.";
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

            this.LoadNextDays(data.list);
        }

        public void LoadNextDays(List_Weather_API[] data)
        {
            this.NextDays.Clear();

            this.NextDays.Add(WeatherDayVM.ObjectToVm(data[8]));
            this.NextDays.Add(WeatherDayVM.ObjectToVm(data[16]));
            this.NextDays.Add(WeatherDayVM.ObjectToVm(data[24]));
            this.NextDays.Add(WeatherDayVM.ObjectToVm(data[32]));

            //Test.ItemsSource = this.NextDays;

        }

        private async void FindPostalCode(object sender, RoutedEventArgs e)
        {
            var result = await OpenWeatherAPI.GetWeatherDataFromZipCode(PostalCodeInput.Text);
            if (result != null) this.LoadData(result);
        }
    }
}
