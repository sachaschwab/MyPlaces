using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using System.Diagnostics;

namespace MyPlaces.Standard.Data
{
    public class DataAccessLayer
    {
        private static List<Place> places = new List<Place>();
        private static List<Category> categories = new List<Category>();

        protected SQLiteAsyncConnection connection;

        public static string DbFolder { get; set; }

        private static string dbFile = "photos.db";

        static DataAccessLayer()
        {
            places.Add(new Place { PlaceId = 1, CategoryId = 1, Title = "Haus", Description = "Leider nicht meins", Date = DateTime.Parse("2017-10-01"), Latitude = 47.2, Longitude = 8.8, Path = "Haus_image.png"  });
            places.Add(new Place { PlaceId = 2, CategoryId = 2, Title = "Schiff", Description = "Leider nicht meins", Date = DateTime.Parse("2017-10-02"), Latitude = 47.3, Longitude = 8.81, Path = "Haus_image.png"  });

            categories.Add(new Category { CategoryId = 1, Name = "Häuser", Color = "#FF0000" });
            categories.Add(new Category { CategoryId = 2, Name = "Schiffe", Color = "#00FF00" });
        }

        public DataAccessLayer()
        {
            connection = new SQLiteAsyncConnection(Path.Combine(DbFolder, dbFile));
        }

        public static void InitializeDb()
        {
            using (SQLiteConnection cnn = new SQLiteConnection(Path.Combine(DbFolder, dbFile)))
            {
                Debug.WriteLine($"### DBFolder: {DbFolder}");
                // make sure tables exist
                cnn.CreateTable<Place>();
                cnn.CreateTable<Category>();

                // add some test-data if necessary
                if (!cnn.Table<Place>().Any())
                    cnn.InsertAll(places);

                if (!cnn.Table<Category>().Any())
                    cnn.InsertAll(categories);
            }
        }

        public async Task<List<Place>> GetAllPhotos()
        {
            return await connection.Table<Place>().ToListAsync();
        }

        public async Task<List<Place>> GetAllPhotosByCategoryId(int categoryId)
        {
            return await connection.Table<Place>().Where(photo => photo.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Place> GetPhotoById(int id)
        {
            return await connection.Table<Place>().Where(photo => photo.PlaceId == id).FirstOrDefaultAsync();
        }

        public async Task SavePlace(Place place)
        {
            await connection.InsertOrReplaceAsync(place);
        }

        public async Task AddPhoto(Place photo)
        {
            await connection.InsertAsync(photo);
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await connection.Table<Category>().ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await connection.Table<Category>().Where(category => category.CategoryId == id).FirstOrDefaultAsync();
        }

        public async Task AddCategory(Category category)
        {
            await connection.InsertAsync(category);
        }

        public async Task InsertOrReplaceCategory(Category category)
        {
            await connection.InsertOrReplaceAsync(category);
        }
    }
}
