using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MyPlaces.Standard
{
    public partial class CategoryListPage : ContentPage
    {
        public CategoryListPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                Padding = new Thickness(0, 20, 0, 0);
        }
    }
}
