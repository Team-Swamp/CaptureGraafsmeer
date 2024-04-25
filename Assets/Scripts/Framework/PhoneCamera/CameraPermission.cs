using UnityEngine;
using UnityEngine.Android;

namespace Framework.PhoneCamera
{
    /// <summary>
    /// A class that will ask to use the camera of the device, inherit form this class to insure camera usage.
    /// When doing so make the start class a protected override.
    /// </summary>
    public abstract class CameraPermission : MonoBehaviour, IPermissionAsker
    {
        private const int ASK_PERMISSION_DELAY = 6;
        private const string CAMERA_PERMISSION = Permission.Camera;

        private void Start() => Invoke(nameof(AskPermission), ASK_PERMISSION_DELAY);

        /// <summary>
        /// Will ask the device camera, will be used inherited classes
        /// </summary>
        public void AskPermission()
        {
            if (!Permission.HasUserAuthorizedPermission(CAMERA_PERMISSION))
                Permission.RequestUserPermission(CAMERA_PERMISSION);
        }
    }
}