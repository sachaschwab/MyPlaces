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
        public NewPhotoPage()
        {
            InitializeComponent();

            this.BindingContext = new NewPhotoViewModel();

            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }
    }
}