using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Winplex.models;

namespace Winplex.Utils
{
    class OpenWeatherAPI
    {
        private static string API_KEY = "26d8478f83046b5a06f5610296ab5101";

        public static async Task<Weather_API> GetWeatherData(double latitude, double longitude)
        {
            var http = new HttpClient();
            var response = await http.GetAsync("http://api.openweathermap.org/data/2.5/forecast?lat="+latitude+"&lon="+longitude+"&APPID="+API_KEY);
            var result = await response.Content.ReadAsStringAsync();
            
            var serializer = new DataContractJsonSerializer(typeof(Weather_API));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (Weather_API)serializer.ReadObject(memoryStream);
            return data;
        }
        public static async Task<Weather_API> GetWeatherDataFromZipCode(string postalCode)
        {
            var http = new HttpClient();
            var response = await http.GetAsync("http://api.openweathermap.org/data/2.5/forecast?zip=" + postalCode + ",fr&APPID=" + API_KEY);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                var serializer = new DataContractJsonSerializer(typeof(Weather_API));
                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var data = (Weather_API) serializer.ReadObject(memoryStream);
                return data;
            }
            return null;
        }

    }
}
