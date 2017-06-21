using System;
using System.Linq;

namespace Winplex.models
{
    public class WeatherDayVM
    {
        public string Day { get; set; }

        public string Image { get; set; }

        public string Temperature { get; set; }

        public string Description { get; set; }

        public static WeatherDayVM ObjectToVm(List_Weather_API data)
        {
            var result = new WeatherDayVM();
            result.Image = string.Format("ms-appx:///Assets/icon/{0}.png", data.weather.ElementAt(0).icon);
            result.Temperature = Weather_Main.KelvinToCelsiusString(data.main.temp);
            result.Day = DateTime.Parse(data.dt_txt).DayOfWeek.ToString();
            result.Description = data.weather.ElementAt(0).description;

            return result;
        }
    }
}
