using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MyPlaces.Standard
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.CurrentPageChanged += OnCurrentPageChanged;
        }

        /// <summary>Provide logic when tab / page changes.</summary><param name="sender">Sender.</param>
        /// <param name="eventArgs">Event arguments.</param>
        private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
        {

            if (this.CurrentPage.Title == "New Photo")
            {
                //((App)App.Current).CurrentImagePath = categoryId;
            }
        }
    }
}
