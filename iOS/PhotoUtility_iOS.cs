using System;
using System.Diagnostics;
using System.Drawing;
using MyPlaces.Standard;
using UIKit;

namespace MyPlaces.iOS
{
    public class PhotoUtility_iOS : IPhotoUtility
    {
        public string PhotoBasePath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>Creates a new file with ending thumb.jpg which is smaller</summary>
        /// <param name="path">Absolute path to source image file</param>
        /// <param name="maxSideLength">Max side length.</param>
        public void GenerateThumbnail(string path, int maxSideLength)
        {
            UIImage sourceImage = UIImage.FromFile(path);
            UIImage destImage = MaxResizeImage(sourceImage, (float)maxSideLength, (float)maxSideLength);
            string extension = System.IO.Path.GetExtension(path);
            string destinationPath = path.Substring(0, path.Length - extension.Length) + ".thumb" + extension;
            destImage.AsJPEG(/* TODO: specify compression quality here */).Save(destinationPath, false);
            Debug.WriteLine($"Saved thumbnail to {destinationPath}");
        }

        public UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
    }
}
