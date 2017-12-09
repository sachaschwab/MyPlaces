using System.Windows.Input;
using MyPlaces.Standard.Data;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MyPlaces.Standard.ViewModels
{

    public class PlacesViewModel : BaseViewModel
    {
        private string _categoryButtonText = "Press to select a category";
        DataAccessLayer dataAccessLayer = new Data.DataAccessLayer();
        MapPage mapPage = new MapPage();
        App app = (App)App.Current;

        public ObservableCollection<Place> Places { get; set; }
        public Command LoadPlacesCommand { get; set; }

        public PlacesViewModel()
        {
            Places = new ObservableCollection<Place>();
            // LoadPlacesCommand = new Command(async () => await ExecuteLoadPlacesCommand());
            LoadData().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    throw t.Exception;
            });

            MessagingCenter.Subscribe<object>(this, MessageNames.CATEGORY_EDITED, _ => {
                LoadData().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;
                });
            });
        }

        public string CategoryButtonText
        {
            get
            {
                return _categoryButtonText;
            }
            set
            {
                _categoryButtonText = value;
                OnPropertyChanged(nameof(CategoryButtonText));
            }
        }

        private async Task LoadData()
        {
            Categories = await dataAccessLayer.GetAllCategories();
        }

        private async Task RefreshPlaces()
        {
            var test = await dataAccessLayer.GetAllPlaces();

            Places.Clear();
            if (selectedCategory != null)
            {
                var result = await dataAccessLayer.GetAllPlacesByCategoryId(selectedCategory.CategoryId.Value);
                foreach (Place place in result)
                    Places.Add(place);
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get => selectedCategory;
            set {
                app.SelectedCategory = value;
                app.SelectedPlaceId = null;
                if (selectedCategory?.CategoryId == value?.CategoryId)
                    return;
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                if (selectedCategory == null)
                {
                    CategoryButtonText = "Press to select a category";
                }
                else
                {
                    CategoryButtonText = selectedCategory.Name;
                }
                RefreshPlaces().ContinueWith(t => { if (t.IsFaulted) throw t.Exception; });
            }
        }

        private List<Category> categories;
        public List<Category> Categories
        {
            get => categories;
            set {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await RefreshPlaces();

                    IsRefreshing = false;
                });
            }
        }  

        private ICommand addPlaceCommand;
        public ICommand AddPlaceCommand
        {
            get {
                if (addPlaceCommand == null)
                {
                    addPlaceCommand = new Command(async () => {
                        MainPage mainPage = (MainPage)App.Current.MainPage;
                        NewPhotoPage newPhotoPage = mainPage.Children.OfType<NewPhotoPage>().First();
                        await ((NewPhotoViewModel)newPhotoPage.BindingContext).PrepareForNewPlace();
                        mainPage.CurrentPage = newPhotoPage;
                    });
                }
                return addPlaceCommand;
            }
        }
    }
}
