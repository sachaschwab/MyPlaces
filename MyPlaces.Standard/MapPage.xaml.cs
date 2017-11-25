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
            var photos = await dataAccessLayer.GetAllPhotos();

            Console.WriteLine("Photo Title = {0}", photos[0].Title);

            Karte.Pins.Clear();

            var photo1 = photos[0];
            var photo2 = photos[1];

            var biggestLat = 47.5;
            var lowestLat = 47.0;
            var biggestLong = 8.9;
            var lowestLong = 8.8;

            var distance = Data.DistanceCalculator.GetDistanceInMetres(photo1.Latitude, photo1.Longitude, photo2.Latitude, photo2.Longitude);
            Console.WriteLine("Distance = {0}", distance);

            Karte.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(biggestLat - ((biggestLat - lowestLat)/2), biggestLong - ((biggestLong - lowestLong) / 2)),

                Distance.FromMeters(distance)));

            foreach (var photo in photos)
            {
                var pin = new Pin
                {
                    Type = PinType.Generic,
                    Position = new Position(photo.Latitude, photo.Longitude),
                    Label = photo.Title,
                    //Address = "HSR",

                };

                Karte.Pins.Add(pin);

            }
        }
    }
}
