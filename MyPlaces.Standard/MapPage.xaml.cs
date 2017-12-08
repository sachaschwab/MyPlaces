using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyPlaces.Standard.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using MyPlaces.Standard.Data;

namespace MyPlaces.Standard
{
    public partial class MapPage : ContentPage
    {
        Data.DataAccessLayer dataAccessLayer = new Data.DataAccessLayer();
        Data.MapCenter mapCenter = new Data.MapCenter();
        // int currentCategoryId;
        List<Place> places = new List<Place>();
        App app = (App)App.Current;

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
            places.Clear();
            Karte.Pins.Clear();

            TitleLabel.IsVisible = app.SelectedPlaceId.HasValue;

            if (app.SelectedPlaceId.HasValue)
            {
                Place place = await dataAccessLayer.GetPlaceById(app.SelectedPlaceId.Value);
                TitleLabel.Text = place.Title;
                places.Add(place);
            }
            else if (app.SelectedCategory != null)
            {
                places = await dataAccessLayer.GetAllPlacesByCategoryId(app.SelectedCategory.CategoryId.Value);
            }
            else
                places = await dataAccessLayer.GetAllPlaces();

            PresentMap(places);
        }

        private void PresentMap(List<Data.Place> places)
        {
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
            foreach (var place in places.Where(p => p.Latitude != 0 && p.Longitude != 0)) // in case location permission is denied, we save places without coorinates.
            {
                var pin = GetPin(place);
                Karte.Pins.Add(pin);
            }
        }

        public Pin GetPin(Place place)
        {
            var pin = new Pin
            {
                Type = PinType.Generic,
                Position = new Position(place.Latitude, place.Longitude),
                Label = place.Title //  + Environment.NewLine + "(" + place.CategoryId + ")"
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
