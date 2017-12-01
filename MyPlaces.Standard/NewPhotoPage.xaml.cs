using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Media;

using MyPlaces.Standard.Data;
using System.Linq;


namespace MyPlaces.Standard
{
    public partial class NewPhotoPage : ContentPage
    {
        private bool isEditable = true;// false

        private DataAccessLayer AccessLayer = new DataAccessLayer();

        public NewPhotoPage()
        {
            InitializeComponent();

            // for testing only!!!!
            SetEditable(isEditable);

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            CameraButton.Clicked += TakeAPictureAsync;
            SaveButton.Clicked += SavePlace;
        }

        public NewPhotoPage(bool EditState)
        {
            InitializeComponent();

            isEditable = EditState;

            SetEditable(isEditable);
        }

        private async void TakeAPictureAsync(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null) return;

            await DisplayAlert("File Location", file.Path, "OK");

            var image = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            DisplayImage(image);
            GetLocation();
            SetDateTime();
        }

        private void SavePlace(object sender, EventArgs e)
        {
            Place NewPlace = new Place();

        }

        private void DisplayImage(ImageSource imageSource)
        {
            Foto.Source = imageSource;


        }

        private void SetDateTime()
        {
            DateTimeLabel.Text = DateTime.Now.ToString();
        }

        private void SetEditable(bool editable)
        {
            if (editable)
            {
                CameraButton.IsVisible = true;
                TitleText.IsEnabled = true;
                NoteText.IsEnabled = true;
                CategoryPicker.IsVisible = true;
                CategoryLabel.IsVisible = false;
                SaveButton.IsVisible = true;

                GetCategoryData();
            }
            else
            {
                CameraButton.IsVisible = false;
                TitleText.IsEnabled = false;
                NoteText.IsEnabled = false;
                CategoryPicker.IsVisible = false;
                CategoryLabel.IsVisible = true;
                SaveButton.IsVisible = false;
            }
        }

        private async Task GetCategoryData()
        {
            List<Category> categories = await AccessLayer.GetAllCategories();

            CategoryPicker.ItemsSource = categories;
            CategoryPicker.ItemDisplayBinding = new Binding("Name");
            CategoryPicker.SelectedIndex = 1;
        }

        private async Task GetLocation()
        {
            if(App.LocationPermission)
            {
                
            }
        }

    }
}