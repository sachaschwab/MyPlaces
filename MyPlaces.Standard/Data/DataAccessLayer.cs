using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlaces.Standard.Data
{
    public class DataAccessLayer
    {
        private static List<Photo> photos = new List<Photo>();
        private static List<Category> categories = new List<Category>();

        static DataAccessLayer()
        {
            photos.Add(new Photo { PhotoId = 1, CategoryId = 1, Title = "Haus", Description = "Leider nicht meins", Date = DateTime.Parse("2017-10-01"), Latitude = 47.2, Longitude = 8.8 });
            photos.Add(new Photo { PhotoId = 2, CategoryId = 2, Title = "Schiff", Description = "Leider nicht meins", Date = DateTime.Parse("2017-10-02"), Latitude = 47.5, Longitude = 8.3 });

            categories.Add(new Category { CategoryId = 1, Name = "Häuser", Color = "#FF0000" });
            categories.Add(new Category { CategoryId = 2, Name = "Schiffe", Color = "#00FF00" });
        }

        public async Task<List<Photo>> GetAllPhotos()
        {
            await Task.Delay(100);
            return photos;
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            await Task.Delay(100);
            // return photos.Single((e) => { return e.Id == id; });
            return photos.Single(photo => photo.PhotoId == id);
        }

        public async Task AddPhoto(Photo photo)
        {
            await Task.Delay(100);
            photos.Add(photo);
        }

        public async Task<List<Category>> GetAllCategories()
        {
            await Task.Delay(100);
            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            await Task.Delay(100);
            return categories.Single(category => category.CategoryId == id);
        }

        public async Task AddCategory(Category category)
        {
            await Task.Delay(100);
            categories.Add(category);
        }
    }
}
