using System;
using System.Windows.Input;
using MyPlaces.Standard.Data;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MyPlaces.Standard.ViewModels
{

    public class PlacesViewModel : BaseViewModel
    {
        private string _categoryButtonText = "Press to select a category";
        Data.DataAccessLayer dataAccessLayer = new Data.DataAccessLayer();
        MapPage mapPage = new MapPage();

        public ObservableCollection<Place> Places { get; set; }
        public Command LoadPlacesCommand { get; set; }

        public PlacesViewModel()
        {
            Places = new ObservableCollection<Place>();
            LoadPlacesCommand = new Command(async () => await ExecuteLoadPlacesCommand());
            LoadData().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    throw t.Exception;
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
            Places.Clear();
            if (selectedCategory != null)
            {
                var result = await dataAccessLayer.GetAllPhotosByCategoryId(selectedCategory.CategoryId.Value);
                foreach (Place place in result)
                    Places.Add(place);
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get => selectedCategory;
            set {
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

        async Task ExecuteLoadPlacesCommand()
        {
            Places.Clear();

            if (SelectedCategory == null)
                return; 
            // var categoryId = ((App)App.Current).SelectedCategory?.CategoryId ?? 1;

            var places = await dataAccessLayer.GetAllPhotosByCategoryId(SelectedCategory.CategoryId.Value);

            foreach (var place in places)
            {
                Places.Add(place);
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
