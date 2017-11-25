using MyPlaces.Standard;
using MyPlaces.Standard.Data;
using Xamarin.Forms;

namespace MyPlaces
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DataAccessLayer.InitializeDb();
            MainPage = new MainPage();
        }

        public static IPhotoUtility PhotoUtility { get; set; }

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
