using MyPlaces.Standard;
using MyPlaces.Standard.Data;
using MyPlaces.Standard.ViewModels;
using Xamarin.Forms;

namespace MyPlaces
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DataAccessLayer.InitializeDb();
            MainPage = new MainPage { BindingContext = new MainPageViewModel() };
        }

        public static IPhotoUtility PhotoUtility { get; set; }

        /// <summary>Null means, there is no category selected.</summary>
        public int? CurrentCategoryID { get; set; }

        /// <summary>Null means, there is no selected place.</summary>
        public int? SelectedPlaceId { get; set; }

        public IPermission Permissions { get; set; }

        public static bool LocationPermission = false;

        protected override void OnStart()
        {
            if (Permissions.HasLocationPermission)
                Permissions.RequestLocationPermission();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
