using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

namespace Player
{
    public class LocationUpdater : MonoBehaviour
    {
        private const float SCALE_FACTOR = 100;
        
        [SerializeField] private MeshRenderer rend;
        [SerializeField] private Material[] mats;
        
        private InputAction _locationAction;

        private void OnEnable()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                Permission.RequestUserPermission(Permission.CoarseLocation);
            }

            _locationAction = new InputAction(binding: "<Device>/location");
            
            _locationAction.started += _ => Input.location.Start();
            _locationAction.canceled += _ => Input.location.Stop();
            _locationAction.Enable();
        }

        private void OnDisable()
        {
            _locationAction.started -= _ => Input.location.Start();
            _locationAction.canceled -= _ => Input.location.Stop();
            _locationAction.Disable();
        }

        private void Update()
        {
            switch (Input.location.status)
            {
                case LocationServiceStatus.Running:
                {
                    rend.material = mats[0];
                    LocationInfo location = Input.location.lastData;
                    rend.transform.position = new Vector3(location.latitude * SCALE_FACTOR, 0, location.longitude * SCALE_FACTOR);
                    break;
                }
                case LocationServiceStatus.Failed:
                    rend.material = mats[1];
                    break;
                case LocationServiceStatus.Initializing:
                    rend.material = mats[2];
                    break;
                case LocationServiceStatus.Stopped:
                    rend.material = mats[3];
                    break;
            }
        }
    }
}