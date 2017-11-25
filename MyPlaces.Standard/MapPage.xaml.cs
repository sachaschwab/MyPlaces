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

            Karte.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(47.2, 8.8),

                Distance.FromKilometers(20)));

            foreach (var photo in photos)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(photo.Latitude, photo.Longitude),
                    Label = photo.Title,
                    //Address = "HSR",
                };

                Karte.Pins.Add(pin);

            }
        }
    }
}
