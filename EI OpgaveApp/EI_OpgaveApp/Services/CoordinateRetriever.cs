using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Services
{
    public class CoordinateRetriever
    {
        double lat;
        double lon;
        private static CoordinateRetriever coordinateRetriever;
        public static CoordinateRetriever GetInstance
        {
            get
            {
                if (coordinateRetriever == null)
                {
                    coordinateRetriever = new CoordinateRetriever();
                }
                return coordinateRetriever;
            }

        }

        public async Task<List<double>> GetCoordinates()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                lat = position.Latitude;
                lon = position.Longitude;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
            return new List<double>() { lat, lon };
        }
    }
}
