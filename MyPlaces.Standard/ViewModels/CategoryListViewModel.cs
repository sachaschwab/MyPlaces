using System.Collections.Generic;
using System.Threading.Tasks;
using MyPlaces.Standard.Data;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;

namespace MyPlaces.Standard.ViewModels
{
    public class CategoryListViewModel : BaseViewModel
    {
        private DataAccessLayer dal = new DataAccessLayer();

        public EditCategoryViewModel EditCategoryViewModel { get; set; } = new EditCategoryViewModel();

        public CategoryListViewModel()
        {
            LoadData().ContinueWith(t => {
                if (t.IsFaulted) // make sure Excption gets noticed (await not possible in Constructor)
                {
                    Device.BeginInvokeOnMainThread(() => throw t.Exception);
                }
            });
            MessagingCenter.Subscribe<object>(this, MessageNames.CATEGORY_EDITED, async _ => await LoadData());
        }

        private async Task LoadData()
        {
            List<Category> result = await dal.GetAllCategories();
            Categories = result.Select(e => new CategoryViewModel(e)).ToList();
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

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get 
            {
                if (addCategoryCommand == null)
                {
                    addCategoryCommand = new Command(() => {
                        // EditCategoryViewModel.Category = new Category { Name = "Neue Kategorie" };
                        MessagingCenter.Send<object, Category>(this, MessageNames.EDIT_CATEGORY, new Category { Name = "Neue Kategorie", Color = "#FFFFFF" });
                    });
                }
                return addCategoryCommand;
            }
        }
    }
}
