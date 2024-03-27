using UnityEngine;
using UnityEngine.Android;

namespace Framework.GeoLocation
{
    public abstract class LocationPermission : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                return;
           
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
    }
}