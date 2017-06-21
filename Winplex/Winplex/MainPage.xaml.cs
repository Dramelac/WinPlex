using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Winplex.DAL;
using Winplex.models;
using Winplex.Utils;

namespace Winplex
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
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

        private Geolocalisation Geolocalisation { get; set; }

        public ObservableCollection<WeatherDayVM> NextDays { get; set; }

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

                var data = await OpenWeatherAPI.GetWeatherData(pos.Coordinate.Point.Position.Latitude,
                    pos.Coordinate.Point.Position.Longitude);
                LoadData(data);
            }
            else
            {
                FailGeoloc();
            }
        }

        public void FailGeoloc()
        {
            MainTitle.Text = "Location is not available. Please choose a city.";
        }

        public void LoadData(Weather_API data)
        {
            MainTitle.Text = "Today";

            MainCity.Text = data.city.name;
            MainWeather.Text = data.list.ElementAt(0).weather.ElementAt(0).main;
            MainDesc.Text = data.list.ElementAt(0).weather.ElementAt(0).description;
            MainDeg.Text = Weather_Main.KelvinToCelsiusString(data.list.ElementAt(0).main.temp);

            //Weather Image
            var icon = string.Format("ms-appx:///Assets/icon/{0}.png", data.list.ElementAt(0).weather.ElementAt(0).icon);
            MainImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));

            //Wind arrow
            MainWind.Visibility = Visibility.Visible;
            MainWind.RenderTransform = new RotateTransform
            {
                Angle = data.list.ElementAt(0).wind.deg,
                CenterX = 50,
                CenterY = 50
            };
            MainWindSpeed.Text = "Wind speed : " + data.list.ElementAt(0).wind.speed + " m/s";

            LoadNextDays(data.list);
        }

        public void LoadNextDays(List_Weather_API[] data)
        {
            NextDays.Clear();

            NextDays.Add(WeatherDayVM.ObjectToVm(data[8]));
            NextDays.Add(WeatherDayVM.ObjectToVm(data[16]));
            NextDays.Add(WeatherDayVM.ObjectToVm(data[24]));
            NextDays.Add(WeatherDayVM.ObjectToVm(data[32]));
        }

        private async void FindPostalCode(object sender, RoutedEventArgs e)
        {
            var result = await OpenWeatherAPI.GetWeatherDataFromZipCode(PostalCodeInput.Text);
            if (result != null) LoadData(result);
        }
    }
}