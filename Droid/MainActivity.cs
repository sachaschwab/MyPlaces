using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MyPlaces.Standard.Data;
using Android;

namespace MyPlaces.Droid
{
    [Activity(Label = "MyPlaces.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            DataAccessLayer.DbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            

            LoadApplication(new App());
        }

        public void RequestLocationPermission()
        {
            RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 1);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode != 1)
                return;

            if (grantResults[0] == Permission.Denied)
            {
                App.LocationPermission = false;
            }else{
                App.LocationPermission = true;
            }
                
        }
    }
}
