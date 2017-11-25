using System;
using SQLite;

namespace MyPlaces.Standard.Data
{
    public class Category
    {
        [PrimaryKey] [AutoIncrement]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
