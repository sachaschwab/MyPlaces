using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyPlaces.Standard.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;

namespace MyPlaces.Standard
{
    public partial class MapPage : ContentPage
    {
        Data.DataAccessLayer dataAccessLayer = new Data.DataAccessLayer();
        Data.MapCenter mapCenter = new Data.MapCenter();
        int currentCategoryId;

        public MapPage()
        {
            InitializeComponent();

            // Put in padding if iOS
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Check whether the current category ID has been set. Otherwise, set categoryId = 0
            if (((App)App.Current).SelectedCategory != null)
            {
                currentCategoryId = ((App)App.Current).SelectedCategory.CategoryId.Value; 
            }

            List<Data.Place> places;
            places = await GetPlacesList();

            PresentMap(places);
        }

        /* Here, other pages can provide a category to display in background or at MapView call.
         * Category 0 provides places of all Categories
         */
        public async Task SetMapPinCategoryAsync(int categoryId)
        {
            
            currentCategoryId = categoryId;
            List<Data.Place> places;
            places = await GetPlacesList();

            PresentMap(places);
        }

        public async Task<List<Data.Place>> GetPlacesList()
        {
            List<Data.Place> places;

            /* Here we transition from the category list page if the current category is dummy.
             * Otherwise, we transition from a specific category places view.
             * In the dummy case, generate list of places of ALL categories.
             * Otherwise, list places of the specific category.
             */
            if (currentCategoryId != 0)
            {
                places = await dataAccessLayer.GetAllPhotosByCategoryId(currentCategoryId);
            }
            else
            {
                places = await dataAccessLayer.GetAllPhotos();
            }

            return places;
        }

        public void PresentMap(List<Data.Place> places)
        {
            Karte.Pins.Clear();

            if(places.Count == 0)
            {
                return;
            }

            // From Photo List, calculate extreme poin coordinates and center coordinates
            var extrCoords = new Data.MapCenter.ExtremeCoords();
            extrCoords = mapCenter.CalculateExtremeCoords(places);

            var centerCoords = new Data.MapCenter.CenterCoords();
            centerCoords = mapCenter.CalculateMapCenter(extrCoords);

            // With extreme points coordinates, set the span / radius of the map
            var distance = Data.DistanceCalculator.GetDistanceInMetres(extrCoords.BiggestLat, extrCoords.BiggestLong,
                                                                       extrCoords.SmallestLat, extrCoords.SmallestLong);

            // Define minimum distance so not to zoom in 'too much' if e.g. there is only one pin
            if (distance <= 1000)
            {
                distance = 1000;
            }
            // With center coordinates, set the center map point
            Karte.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(centerCoords.CentreLat, centerCoords.CentreLong),
                Distance.FromMeters(distance)));

            // Set the pins
            foreach (var place in places.Where(p => p.Latitude != 0 && p.Longitude != 0))
            {
                var pin = GetPin(place);
                Karte.Pins.Add(pin);
            }
        }

        public Pin GetPin(Data.Place place)
        {
            var pin = new Pin
            {
                Type = PinType.Generic,
                Position = new Position(place.Latitude, place.Longitude),
                Label = place.Title + Environment.NewLine + "(" + place.CategoryId + ")"
            };
            pin.Clicked += async (object sender, EventArgs e) =>
            {
                if (place == null)
                    return;

                var mainPage = this.Parent as TabbedPage;


                // Provide selected place ID to App "dispatch".
                ((App)App.Current).SelectedPlaceId = place.PlaceId;

                var newPhotoPage = mainPage.Children.OfType<NewPhotoPage>().First();
                NewPhotoViewModel vm = (NewPhotoViewModel)newPhotoPage.BindingContext;
                await vm.SetPlace(place.PlaceId.Value);
                mainPage.CurrentPage = newPhotoPage;
            };
            return pin;
        }

    }
}
