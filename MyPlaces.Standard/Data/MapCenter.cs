using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPlaces.Standard.Data
{
    public class MapCenter
    {
        public class ExtremeCoords
        {
            public Double BiggestLat { get; set; }
            public Double BiggestLong { get; set; }
            public Double SmallestLat { get; set; }
            public Double SmallestLong { get; set; }
        }

        public class CenterCoords
        {
            public Double CentreLat { get; set; }
            public Double CentreLong { get; set; }
        }

        public ExtremeCoords CalculateExtremeCoords(List<Place> places)
        {
            var extremeCoords = new ExtremeCoords();

            // Extract extreme photo coordinates
            extremeCoords.SmallestLat = places.Min(p => p.Latitude);
            extremeCoords.BiggestLat = places.Max(p => p.Latitude);
            extremeCoords.SmallestLong = places.Min(p => p.Longitude);
            extremeCoords.BiggestLong = places.Max(p => p.Longitude);

            return extremeCoords;
        }

        public CenterCoords CalculateMapCenter(ExtremeCoords extremeCoords)
        {
            var center = new CenterCoords();

            // Map the centre coordinates
            center.CentreLat = (extremeCoords.BiggestLat + extremeCoords.SmallestLat) / 2;
            center.CentreLong = (extremeCoords.BiggestLong + extremeCoords.SmallestLong) / 2;
            return center;
        }
    }
}