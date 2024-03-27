using UnityEngine;
using UnityEngine.Android;

namespace Framework.PhoneCamera
{
    public abstract class CameraPermission : MonoBehaviour
    {
        private const string CAMERA_PERMISSION = Permission.Camera;

        protected virtual void OnEnable()
        {
            if (!Permission.HasUserAuthorizedPermission(CAMERA_PERMISSION))
                Permission.RequestUserPermission(CAMERA_PERMISSION);
        }
    }
}