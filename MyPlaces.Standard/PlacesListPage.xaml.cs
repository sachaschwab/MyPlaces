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
            // TODO: Dummy => get Category Name from Transition from Category List Page
            Title = "Häuser";

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

            // TODO: => Get category ID from transition from selected category in Category List Page
            var categoryId = 1;
            ((App)App.Current).CurrentCategoryID = categoryId;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var place = args.SelectedItem as Data.Place;
            if (place == null)
                return;
            
        }
    }
}