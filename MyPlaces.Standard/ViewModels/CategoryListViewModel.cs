using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyPlaces.Standard.Data;
using System.Linq;
using Xamarin.Forms;

namespace MyPlaces.Standard.ViewModels
{
    public class CategoryListViewModel : BaseViewModel
    {
        private DataAccessLayer dal = new DataAccessLayer();

        public EditCategoryViewModel EditCategoryViewModel { get; set; } = new EditCategoryViewModel();

        public CategoryListViewModel()
        {
            LoadData();
            MessagingCenter.Subscribe<object>(this, MessageNames.CATEGORY_EDITED, async _ => await LoadData());
        }

        private async Task LoadData()
        {
            List<Category> result = await dal.GetAllCategories();
            Categories = result.Select(e => new CategoryViewModel(e)).ToList();
            // EditCategoryViewModel.Category = Categories.FirstOrDefault();
        }

        private List<CategoryViewModel> categories;
        public List<CategoryViewModel> Categories 
        { 
            get { return categories; }
            set {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }
}
