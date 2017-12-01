using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;



namespace MyPlaces.Standard
{
    public partial class MapPage : ContentPage
    {
        ViewModels.MapViewModel viewModel;
        Data.DataAccessLayer dataAccessLayer = new Data.DataAccessLayer();
        Data.MapCenter mapCenter = new Data.MapCenter();

        public MapPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            BindingContext = viewModel = new ViewModels.MapViewModel();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var places = await dataAccessLayer.GetAllPhotos();

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
