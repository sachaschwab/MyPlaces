using System;
namespace MyPlaces.Standard
{
    public interface IPhotoUtility
    {
        void GenerateThumbnail(string path, int maxSideLength);
        string PhotoBasePath { get; }
    }
}
