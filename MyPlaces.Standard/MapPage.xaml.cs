using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MyPlaces.Standard
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            Karte.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(47.2171656, 8.823212),
                Distance.FromMiles(0.5)));

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(47.2171656, 8.823212),
                Label = "Demo Maps",
                Address = "HSR",
            };

            Karte.Pins.Add(pin);
        }
    }
}
