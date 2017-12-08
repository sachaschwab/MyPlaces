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
        protected SQLiteAsyncConnection connection;

        public static string DbFolder { get; set; }

        private static string dbFile = "photos.db";

        public DataAccessLayer()
        {
            connection = new SQLiteAsyncConnection(Path.Combine(DbFolder, dbFile));
        }

        public static void InitializeDb()
        {
            // must run synchronously
            using (SQLiteConnection cnn = new SQLiteConnection(Path.Combine(DbFolder, dbFile)))
            {
                Debug.WriteLine($"### DBFolder: {DbFolder}");
                // make sure tables exist
                cnn.CreateTable<Place>();
                cnn.CreateTable<Category>();
            }
        }

        public async Task<List<Place>> GetAllPlaces()
        {
            return await connection.Table<Place>().ToListAsync();
        }

        public async Task<List<Place>> GetAllPlacesByCategoryId(int categoryId)
        {
            return await connection.Table<Place>().Where(photo => photo.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Place> GetPlaceById(int id)
        {
            return await connection.Table<Place>().Where(photo => photo.PlaceId == id).FirstOrDefaultAsync();
        }

        public async Task SavePlace(Place place)
        {
            await connection.InsertOrReplaceAsync(place);
        }

        public async Task AddPlace(Place photo)
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
