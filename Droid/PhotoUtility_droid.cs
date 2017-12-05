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

        public void GenerateThumbnail(string path, int maxSideLength)
        {
            Bitmap bitmap = BitmapFactory.DecodeFile(path);
            double factor = (double)maxSideLength / new int[] { bitmap.Height, bitmap.Width }.Max();;
            Bitmap bitmapScaled = null;
            try
            {
                bitmapScaled = Bitmap.CreateScaledBitmap(bitmap, (int)(bitmap.Width * factor), (int)(bitmap.Height * factor), false);
            }
            catch (Exception ex)
            {
                var x = 1;
            }

            string extension = System.IO.Path.GetExtension(path);
            string thumbNailPath = path.Substring(0, path.Length - extension.Length) + ".thumb" + extension;

            using (Stream fileStream = new FileStream(thumbNailPath, FileMode.Create))
            {
                bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, fileStream);
            } 
            bitmap.Recycle();
        }

        public void GenerateThumbnailNew(string path, int maxSideLength)
        {
            try
            {
                Bitmap bitmap = BitmapFactory.DecodeFile(path);
                var factor = new int[] { bitmap.Height, bitmap.Width }.Max() / maxSideLength;
                Bitmap bitmapScaled = Bitmap.CreateBitmap(bitmap.Width * factor, bitmap.Height * factor, Bitmap.Config.Argb8888);
                Canvas c = new Canvas(bitmapScaled);
                c.DrawBitmap(bitmap, bitmap.Width * factor, bitmap.Height * factor, null);


                //try
                //{
                //    bitmapScaled = Bitmap.CreateScaledBitmap(bitmap, bitmap.Width * factor, bitmap.Height * factor, true);
                //}
                //catch (Exception ex)
                //{
                //    var x = 1;
                //}

                string extension = System.IO.Path.GetExtension(path);
                string thumbNailPath = path.Substring(0, path.Length - extension.Length) + ".thumb" + extension;

                using (Stream fileStream = new FileStream(thumbNailPath, FileMode.Create))
                {
                    bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, fileStream);
                }
                bitmap.Recycle();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                var x = 1;
            }
        }
    }
}
