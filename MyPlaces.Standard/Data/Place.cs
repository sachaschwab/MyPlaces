using System;
using SQLite;

namespace MyPlaces.Standard.Data
{
    public class Place
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PhotoId { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        [Ignore]
        public string ThumbNailPath {
            get 
            {
                if (string.IsNullOrWhiteSpace(Path))
                    return "";
                string extension = System.IO.Path.GetExtension(Path);
                return Path.Substring(0, Path.Length - extension.Length) + ".thumb" + extension; 
            }
        }

        public int CategoryId { get; set; }
    }
}
