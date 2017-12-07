using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Media;

using MyPlaces.Standard.Data;
using System.Linq;
using MyPlaces.Standard.ViewModels;

namespace MyPlaces.Standard
{
    public partial class NewPhotoPage : ContentPage
    {
        //private string _imagePath;
        //private DateTime _dateTime;

        //private DataAccessLayer AccessLayer = new DataAccessLayer();

        public NewPhotoPage()
        {
            InitializeComponent();

            this.BindingContext = new NewPhotoViewModel();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            //CameraButton.Clicked += TakeAPictureAsync;
            //SaveButton.Clicked += SavePlace;
        }


        //private async void TakeAPictureAsync(object sender, EventArgs e)
        //{
        //    await CrossMedia.Current.Initialize();

        //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        //    {
        //        await DisplayAlert("No Camera", ":( No camera available.", "OK");
        //        return;
        //    }

        //    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        //    {
        //        //Directory = "Sample",
        //        Name = "test.jpg"
        //    });

        //    if (file == null) return;

        //    _imagePath = file.Path;
        //    //await DisplayAlert("File Location", file.Path, "OK");

        //    var image = ImageSource.FromStream(() =>
        //    {
        //        var stream = file.GetStream();
        //        file.Dispose();
        //        return stream;
        //    });

        //    DisplayImage(image);
        //    GetLocation();
        //    SetDateTime();
        //}

        //private async void SavePlace(object sender, EventArgs e)
        //{
        //    Place newPlace = new Place();
        //    newPlace.Path = _imagePath;
        //    newPlace.Title = TitleText.Text;
        //    newPlace.Description = DescriptionText.Text;
        //    newPlace.Date = _dateTime;
        //    newPlace.CategoryId = ((Category)CategoryPicker.SelectedItem).CategoryId;
        //    //newPlace.Latitude = ;
        //    //newPlace.Longitude = ;

        //    App.PhotoUtility.GenerateThumbnail(_imagePath, 50);

        //    await AccessLayer.AddPlace(newPlace);
        //}

        //private void DisplayImage(ImageSource imageSource)
        //{
        //    Foto.Source = imageSource;
        //}

        //private void SetDateTime()
        //{
        //    _dateTime = DateTime.Now;
        //    DateTimeLabel.Text = _dateTime.ToString();
        //}

        //private void SetEditable(bool editable)
        //{
        //    if (editable)
        //    {
        //        CameraButton.IsVisible = true;
        //        TitleText.IsEnabled = true;
        //        DescriptionText.IsEnabled = true;
        //        CategoryPicker.IsVisible = true;
        //        CategoryLabel.IsVisible = false;
        //        SaveButton.IsVisible = true;

        //        //GetCategoryData();
        //    }
        //    else
        //    {
        //        CameraButton.IsVisible = false;
        //        TitleText.IsEnabled = false;
        //        DescriptionText.IsEnabled = false;
        //        CategoryPicker.IsVisible = false;
        //        CategoryLabel.IsVisible = true;
        //        SaveButton.IsVisible = false;
        //    }
        //}

        //private async Task GetCategoryData()
        //{
        //    List<Category> categories = await AccessLayer.GetAllCategories();

        //    CategoryPicker.ItemsSource = categories;
        //    CategoryPicker.ItemDisplayBinding = new Binding("Name");
        //    CategoryPicker.SelectedIndex = 1;
        //}

        //private async Task GetLocation()
        //{
        //    if(App.LocationPermission)
        //    {
                
        //    }
        //}

    }
}