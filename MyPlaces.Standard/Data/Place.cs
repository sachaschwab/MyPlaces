using System;
using SQLite;

namespace MyPlaces.Standard.Data
{
    public class Place
    {
        [PrimaryKey]
        [AutoIncrement]
        public int? PlaceId { get; set; }

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
                string thumbnailName = Path.Substring(0, Path.Length - extension.Length) + ".thumb" + extension;
                return System.IO.Path.Combine(App.PhotoUtility.PhotoBasePath, thumbnailName);
            }
        }

        public int CategoryId { get; set; }
    }
}
