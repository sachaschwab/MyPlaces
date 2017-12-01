using System;
using System.Collections.Generic;
using MyPlaces.Standard.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;



namespace MyPlaces.Standard
{
    public partial class MapPage : ContentPage
    {
        Data.DataAccessLayer dataAccessLayer = new Data.DataAccessLayer();
        Data.MapCenter mapCenter = new Data.MapCenter();
        ViewModels.MapViewModel mapViewModel;
        Data.Category currentCategory;

        public MapPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            // Category constructor with dummy category = 0
            var category = new Data.Category
            {
                CategoryId = 0,
                Name = "Some name",
                Color = "Red"
            };

            mapViewModel = new MapViewModel(category);
            currentCategory = category;
            BindingContext = mapViewModel;
        }

        public MapPage(MapViewModel mapViewModel)
        {
            InitializeComponent();
            BindingContext = this.mapViewModel = mapViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            List<Data.Place> places;

            /* We come from the category list page if the current category is dummy.
             * Otherwise, we come from a specific category places view.
             * In the dummy case, generate list of places of ALL categories.
             * Otherwise, list places of the specific category.
             */
            if (currentCategory.CategoryId != 0)
            {
                places = await dataAccessLayer.GetAllPhotosByCategoryId(currentCategory.CategoryId);
            }
            else
            {
                places = await dataAccessLayer.GetAllPhotos();
            }

            PresentMap(places);
        }

        public void PresentMap(List<Data.Place> places)
        {
            Karte.Pins.Clear();

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
            foreach (var place in places)
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
                Label = place.Title,
            };
            return pin;
        }

    }
}
