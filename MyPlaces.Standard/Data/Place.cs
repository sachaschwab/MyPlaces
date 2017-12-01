using System;
using SQLite;

namespace MyPlaces.Standard.Data
{
    public class Place
    {
        [PrimaryKey] [AutoIncrement]
        public int PhotoId { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public int CategoryId { get; set; }
    }
}
