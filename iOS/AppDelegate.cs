using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foundation;
using MyPlaces.Standard.Data;
using UIKit;
using Xamarin.Forms.Maps;

namespace MyPlaces.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            DataAccessLayer.DbFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            App.PhotoUtility = new PhotoUtility_iOS();

            UIImage testImage = UIImage.FromBundle("TestImage.png");
            string imgPath = Path.Combine(App.PhotoUtility.PhotoBasePath, "TestImage.png"); 
            testImage.AsJPEG().Save(imgPath, false);
            App.PhotoUtility.GenerateThumbnail(imgPath, 100);

            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init(); 

            LoadApplication(new App());

            ((App)App.Current).Permissions = new Permission_iOS();

            return base.FinishedLaunching(app, options);
        }
    }
}
