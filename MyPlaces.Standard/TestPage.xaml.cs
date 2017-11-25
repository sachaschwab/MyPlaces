using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace MyPlaces.Standard
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            string path = Path.Combine(App.PhotoUtility.PhotoBasePath, "TestImage.png");
            App.PhotoUtility.GenerateThumbnail(path, 100);
        }
    }
}
