using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyPlaces.Standard.Data;
using Xamarin.Forms;

namespace MyPlaces.Standard
{
    public partial class PlacesListPage : ContentPage
    {
        ViewModels.PlacesViewModel viewModel;
        MapPage mapPage = new MapPage();
        private Data.DataAccessLayer AccessLayer = new Data.DataAccessLayer();

        public PlacesListPage()
        {
            InitializeComponent();
            // TODO: Dummy => get Category Name from Transition from Category List Page
            Title = "Places";
            // Put in padding if iOS
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            BindingContext = viewModel = new ViewModels.PlacesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh context upon returning to the page from categories list page
            BindingContext = viewModel = new ViewModels.PlacesViewModel();
            if (viewModel.Places.Count == 0)
                viewModel.LoadPlacesCommand.Execute(null);

            // TODO: Decide whehter to keep the Button bar & picker feature. otherwise, erase these two lines
            CategoryButton.IsVisible = true;
            CategoryPicker.IsVisible = false;
            if (((App)App.Current).SelectedCategory == null)
            {
                CategoryButton.Text = "Please select a category";
            }
            else
            {
                CategoryButton.Text = ((App)App.Current).SelectedCategory.Name;
            }


        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

            var place = args.SelectedItem as Data.Place;
            if (place == null)
                return;

            // Provide selected place ID to App "dispatch".
            ((App)App.Current).SelectedPlaceId = place.PhotoId;

            var mainPage = this.Parent as TabbedPage;
            var newPhotoPage = mainPage.Children[3];
            mainPage.CurrentPage = newPhotoPage;
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            CategoryPicker.IsVisible = false;
            CategoryButton.IsVisible = true;
            GetCategoryData();
            CategoryPicker.Focus();
        }

        private async Task GetCategoryData()
        {
            List<Data.Category> categories = await AccessLayer.GetAllCategories();

            CategoryPicker.ItemsSource = categories;
            CategoryPicker.ItemDisplayBinding = new Binding("Name");
            CategoryPicker.SelectedIndex = ((App)App.Current).SelectedCategory.CategoryId;

            CategoryPicker.Unfocused += (sender, args) =>
            {
                CategoryButton.Text = CategoryPicker.Items[CategoryPicker.SelectedIndex];
                //CategoryButton.Text = CategoryPicker.SelectedIndex.ToString();
                Console.WriteLine("Selected index = {0}", CategoryPicker.SelectedIndex);
                // Refresh the model with the newly chosen category

                ((App)App.Current).SelectedCategory = (Category)CategoryPicker.SelectedItem;
                BindingContext = viewModel = new ViewModels.PlacesViewModel();
                //viewModel.LoadPlacesCommand.CanExecuteChanged;
            };
        }
    }
}
