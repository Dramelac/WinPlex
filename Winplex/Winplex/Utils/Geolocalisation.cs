using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Winplex.Utils
{
    class Geolocalisation
    {
        private Geolocator _geolocator { get; set; }

        public string Status { get; set; }

        public Geolocalisation()
        {
            this.Status = "No initialized";
        }

        public async Task Init()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    _geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
                    this.Status = "Enable";
                    break;

                case GeolocationAccessStatus.Denied:
                    this.Status = "Denied";
                    break;

                case GeolocationAccessStatus.Unspecified:
                    this.Status = "Unknown";
                    break;
            }
            
        }

        

        public async Task<Geoposition> GeoPosition()
        {
            if (_geolocator != null)
            {
                return await _geolocator.GetGeopositionAsync();
            }
            return null;
        }
    }
}
