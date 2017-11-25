using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using System.IO;

namespace MyPlaces.Standard.Data
{
    public class DataAccessLayer
    {
        private static List<Photo> photos = new List<Photo>();
        private static List<Category> categories = new List<Category>();

        protected SQLiteAsyncConnection connection;

        public static string DbFolder { get; set; }

        private static string dbFile = "photos.db";

        static DataAccessLayer()
        {
            photos.Add(new Photo { PhotoId = 1, CategoryId = 1, Title = "Haus", Description = "Leider nicht meins", Date = DateTime.Parse("2017-10-01"), Latitude = 47.2, Longitude = 8.8 });
            photos.Add(new Photo { PhotoId = 2, CategoryId = 2, Title = "Schiff", Description = "Leider nicht meins", Date = DateTime.Parse("2017-10-02"), Latitude = 47.3, Longitude = 8.81 });

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
                // make sure tables exist
                cnn.CreateTable<Photo>();
                cnn.CreateTable<Category>();

                // add some test-data if necessary
                if (!cnn.Table<Photo>().Any())
                    cnn.InsertAll(photos);

                if (!cnn.Table<Category>().Any())
                    cnn.InsertAll(categories);
            }
        }

        public async Task<List<Photo>> GetAllPhotos()
        {
            return await connection.Table<Photo>().ToListAsync();
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await connection.Table<Photo>().Where(photo => photo.PhotoId == id).FirstOrDefaultAsync();
        }

        public async Task AddPhoto(Photo photo)
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
    }
}
