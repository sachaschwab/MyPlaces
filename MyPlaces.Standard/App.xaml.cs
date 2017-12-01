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

        public int CurrentCategoryID { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
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
