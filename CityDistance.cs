using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTunes
{
    class CityDistance
    {
        //Real City Name (in the language of the country)
        public string CityName;
        //Distance in game units
        public double Distance;
        public CityDistance(string cityName, double distance)
        { 
            this.CityName = cityName;
            this.Distance = distance;
        }

        public override string ToString()
        {
            return $"{CityName} — {Distance:F2}";
        }
    }
}
