using System;
using System.Collections.Generic;
using MyPlaces.Standard.Data;
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

            MessagingCenter.Subscribe<object, Category>(this, MessageNames.EDIT_CATEGORY, (sender, cat) => EditControl.IsVisible = true);
        }

        //void Edit_Clicked(object sender, System.EventArgs e)
        //{
        //    EditControl.IsVisible = true;
        //}

        void Save_Clicked(object sender, System.EventArgs e)
        {
            // TODO: save changes
            EditControl.IsVisible = false;
        }

        void Cancel_Clicked(object sender, System.EventArgs e)
        {
            EditControl.IsVisible = false;
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var x = 1;
        }
    }
}
