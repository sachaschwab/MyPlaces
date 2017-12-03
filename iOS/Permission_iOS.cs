using System;
using System.Threading.Tasks;
using AVFoundation;
using CoreLocation;
using MyPlaces.Standard;

namespace MyPlaces.iOS
{
    public class Permission_iOS : IPermission
    {
        private CLLocationManager locManager;

        public Permission_iOS()
        {
            locManager = new CLLocationManager();
            locManager.AuthorizationChanged += (sender, e) => {
                if (locationPermissionTCS.Task.Status != TaskStatus.RanToCompletion)
                    locationPermissionTCS?.SetResult(HasLocationPermission);
            };
        }

        public bool HasLocationPermission => CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways || CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse;

        private TaskCompletionSource<bool> locationPermissionTCS;
        public Task<bool> RequestLocationPermission()
        {
            locationPermissionTCS = new TaskCompletionSource<bool>();
            locManager.RequestWhenInUseAuthorization();
            return locationPermissionTCS.Task;
        }
    }
}