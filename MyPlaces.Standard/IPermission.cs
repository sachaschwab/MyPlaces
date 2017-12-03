using System.Threading.Tasks;

namespace MyPlaces.Standard
{
    public interface IPermission
    {
        bool HasLocationPermission { get; }
        Task<bool> RequestLocationPermission();
    }
}