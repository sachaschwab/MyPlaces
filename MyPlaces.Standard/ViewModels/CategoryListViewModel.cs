using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyPlaces.Standard.Data;

namespace MyPlaces.Standard.ViewModels
{
    public class CategoryListViewModel : BaseViewModel
    {
        private DataAccessLayer dal = new DataAccessLayer();

        public CategoryListViewModel()
        {
            LoadData();
        }

        private async Task LoadData()
        {
            List<Category> result = await dal.GetAllCategories();
            Categories = result;
        }

        private List<Category> categories;
        public List<Category> Categories 
        { 
            get { return categories; }
            set {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }
}
