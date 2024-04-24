using UnityEngine;
using UnityEngine.Android;

namespace Framework.GeoLocation
{
    /// <summary>
    /// A class that will ask to use the location of the device, inherit form this class to insure location usage.
    /// When doing so make the start class a protected override.
    /// </summary>
    public abstract class LocationPermission : MonoBehaviour, IPermissionAsker
    {
        private const int ASK_PERMISSION_DELAY = 3;
        
        protected virtual void Start() => Invoke(nameof(AskPermission), ASK_PERMISSION_DELAY);

        /// <summary>
        /// Will ask the device location, will be used inherited classes
        /// </summary>
        public void AskPermission()
        {
            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                return;
           
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
    }
}