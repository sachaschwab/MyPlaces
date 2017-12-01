using System;
using Android.Graphics;
using MyPlaces.Standard;
using System.Linq;
using Java.IO;
using System.IO;

namespace MyPlaces.Droid
{
    public class PhotoUtility_droid : IPhotoUtility
    {
        public string PhotoBasePath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        void IPhotoUtility.GenerateThumbnail(string path, int maxSideLength)
        {
            Bitmap bitmap = BitmapFactory.DecodeFile(path);
            var factor = new int[] { bitmap.Height, bitmap.Width }.Max() / maxSideLength;
            Bitmap bitmapScaled = Bitmap.CreateScaledBitmap(bitmap, bitmap.Width * factor, bitmap.Height * factor, true);
            bitmap.Recycle();

            string extension = System.IO.Path.GetExtension(path);
            string thumbNailPath = path.Substring(0, path.Length - extension.Length) + ".thumb" + extension;

            using (Stream fileStream = new FileStream(thumbNailPath, FileMode.Create))
            {
                bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, fileStream);
            } 
        }
    }
}
