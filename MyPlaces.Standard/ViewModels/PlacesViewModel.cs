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

        private async Task LoadData()
        {
            Categories = await dataAccessLayer.GetAllCategories();
        }

        private async Task RefreshPlaces()
        {
            // /var/mobile/Containers/Data/Application/D9AFB064-AE85-4BDC-8D1E-0EC66D0B6BA8/Documents/myPlace_1512652612.15464.thumb.jpg
            var fileResult = System.IO.Directory.EnumerateFiles(App.PhotoUtility.PhotoBasePath);
            foreach (string file in fileResult)
                Debug.WriteLine(file);

            Places.Clear();
            if (selectedCategory != null)
            {
                var result = await dataAccessLayer.GetAllPhotosByCategoryId(selectedCategory.CategoryId);
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

            var categoryId = ((App)App.Current).SelectedCategory?.CategoryId ?? 1;

            var places = await dataAccessLayer.GetAllPhotosByCategoryId(categoryId);

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
