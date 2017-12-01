using System;
using System.Collections.Generic;

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

            // Extract photo coordinates into an max-iterable form (array)
            foreach (var place in places)
            {
                if (place.Latitude > extremeCoords.BiggestLat) { extremeCoords.BiggestLat = place.Latitude; }
                if (place.Latitude < extremeCoords.SmallestLat | extremeCoords.SmallestLat == 0)
                {
                    extremeCoords.SmallestLat = place.Latitude;
                }
                if (place.Longitude > extremeCoords.BiggestLong) { extremeCoords.BiggestLong = place.Longitude; }
                if (place.Longitude < extremeCoords.SmallestLong | extremeCoords.SmallestLong == 0)
                {
                    extremeCoords.SmallestLong = place.Longitude;
                }
            }

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