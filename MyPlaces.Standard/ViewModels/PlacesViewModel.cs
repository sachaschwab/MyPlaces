using System;
using System.Windows.Input;
using MyPlaces.Standard.Data;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

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

        }

        async Task ExecuteLoadPlacesCommand()
        {
            
            try
            {
                Places.Clear();

                // Get category ID eventually selected from CategoriesList
                // TODO: Erase this dummy once Doiminik has implemented shift from Categories list page
                var categoryId = ((App)App.Current).SelectedCategory?.CategoryId ?? 1;

                var places = await dataAccessLayer.GetAllPhotosByCategoryId(categoryId);

                foreach (var place in places)
                {
                    Places.Add(place);
                }
            } 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private ICommand addPlaceCommand;
        public ICommand AddPlaceCommand
        {
            get {
                if (addPlaceCommand == null)
                {
                    addPlaceCommand = new Command(() => {
                        MainPage mainPage = (MainPage)App.Current.MainPage;
                        NewPhotoPage newPhotoPage = mainPage.Children.OfType<NewPhotoPage>().First();
                        ((NewPhotoViewModel)newPhotoPage.BindingContext).IsEditable = true;
                        mainPage.SelectedItem = newPhotoPage;
                    });
                }
                return addPlaceCommand;
            }
        }
    }
}
