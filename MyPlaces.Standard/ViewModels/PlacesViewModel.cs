using System;
using System.Windows.Input;
using MyPlaces.Standard.Data;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

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
                var places = await dataAccessLayer.GetAllPhotosByCategoryId(1);

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
    }
}
