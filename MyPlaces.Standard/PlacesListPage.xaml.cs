using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MyPlaces.Standard
{
    public partial class PlacesListPage : ContentPage
    {
        ViewModels.PlacesViewModel viewModel;
        MapPage mapPage = new MapPage();

        public PlacesListPage()
        {
            InitializeComponent();

            Title = "Places";

            // Put in padding if iOS
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            BindingContext = viewModel = new ViewModels.PlacesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Places.Count == 0)
                viewModel.LoadPlacesCommand.Execute(null);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Place details from selection
            var place = args.SelectedItem as Data.Place;
            if (place != null)
            {
                // Provide selected place ID to App "dispatch".
                ((App)App.Current).SelectedPlaceId = place.PhotoId;

                // Switch to New Place page
                var mainPage = this.Parent as TabbedPage;
                var newPhotoPage = mainPage.Children[3];
                mainPage.CurrentPage = newPhotoPage;
            }
            else 
                return;
            
        }
    }
}