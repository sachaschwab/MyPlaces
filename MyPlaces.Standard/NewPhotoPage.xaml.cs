using Xamarin.Forms;

using MyPlaces.Standard.ViewModels;

namespace MyPlaces.Standard
{
    public partial class NewPhotoPage : ContentPage
    {
        public NewPhotoPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }
    }
}