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
using MyPlaces.Standard;
using System.Threading.Tasks;
using System.Linq;

namespace MyPlaces.Droid
{
    [Activity(Label = "MyPlaces.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPermission
    {
        private int LOCATION_PERMISSION_REQUEST = 1;
        private int CAMERA_PERMSISSION_REQUEST = 2;

        public bool HasLocationPermission => CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            App.PhotoUtility = new PhotoUtility_droid();


            DataAccessLayer.DbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
            ((App)App.Current).Permissions = this;
        }

        public void RequestLocationPermission()
        {
            RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 1);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (!grantResults.Any())
                return;
            if (requestCode == LOCATION_PERMISSION_REQUEST)
            {
                localPermissionTCS.SetResult(grantResults[0] == Permission.Granted);
            }
        }

        private TaskCompletionSource<bool> localPermissionTCS;
        Task<bool> IPermission.RequestLocationPermission()
        {
            RequestPermissions(new string[] { Manifest.Permission.AccessFineLocation }, 1);
            localPermissionTCS = new TaskCompletionSource<bool>();
            return localPermissionTCS.Task;
        }
    }
}
