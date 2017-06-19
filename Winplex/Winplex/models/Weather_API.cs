using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Winplex.models
{
    [DataContract]
    public class Weather_API
    {
        [DataMember]
        public string cod { get; set; }

        [DataMember]
        public double message { get; set; }

        [DataMember]
        public City city { get; set; }

        [DataMember]
        public int cnt { get; set; }

        [DataMember]
        public List_Weather_API[] list { get; set; }
    }

    [DataContract]
    public class City
    {
        [DataMember]
        public double id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string country { get; set; }

        [DataMember]
        public Coord coord { get; set; }

        [DataMember]
        public double population { get; set; }
    }

    [DataContract]
    public class Coord
    {
        [DataMember]
        public double lat { get; set; }

        [DataMember]
        public double lon { get; set; }
    }

    [DataContract]
    public class List_Weather_API
    {
        [DataMember]
        public double dt { get; set; }

        [DataMember]
        public string dt_txt { get; set; }

        [DataMember]
        public Weather_Main main { get; set; }

        [DataMember]
        public Weather[] weather { get; set; }

        [DataMember]
        public Cloud clouds { get; set; }

        [DataMember]
        public Wind wind { get; set; }

        [DataMember]
        public Sys sys { get; set; }
    }

    [DataContract]
    public class Weather_Main
    {
        [DataMember]
        public double temp { get; set; }

        [DataMember]
        public double temp_min { get; set; }

        [DataMember]
        public double temp_max { get; set; }

        [DataMember]
        public double pressure { get; set; }

        [DataMember]
        public double sea_level { get; set; }

        [DataMember]
        public double grnd_level { get; set; }

        [DataMember]
        public int humidity { get; set; }

        [DataMember]
        public double temp_kf { get; set; }

        public static double KelvinToCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }

        public static string KelvinToCelsiusString(double kelvin)
        {
            return (kelvin - 273.15).ToString(CultureInfo.CurrentCulture) + " °C";
        }

    }

    [DataContract]
    public class Weather
    {

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string main { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string icon { get; set; }

    }

    [DataContract]
    public class Cloud
    {
        [DataMember]
        public int all { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public double speed { get; set; }

        [DataMember]
        public double deg { get; set; }
    }

    [DataContract]
    public class Sys
    {
        [DataMember]
        public string pod { get; set; }
    }
}
