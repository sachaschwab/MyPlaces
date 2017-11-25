using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Plugin.Media;


namespace MyPlaces.Standard
{
    public partial class NewPhotoPage : ContentPage
    {
        public NewPhotoPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            CameraButton.Clicked += TakeAPictureAsync;

        }

        private async void  TakeAPictureAsync(object sender, EventArgs e)
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


        }

        private void DisplayImage(ImageSource imageSource)
        {
            Foto.Source = imageSource;
        }


    }
}
