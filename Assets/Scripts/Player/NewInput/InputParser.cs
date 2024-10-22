using UnityEngine;
using UnityEngine.InputSystem;

using FrameWork;

namespace Player.NewInput
{
    public sealed class InputParser : MonoBehaviour
    {
        private const string INTERACTABLE_TAG = "Interactable";
        
        [SerializeField, Range(1, 100)] private float interactableRayDistance;
        
        private Camera _mainCamera;
        private PlayerInput _playerInput;
        private InputActionAsset _inputActionAsset;
        private InteractableObject _lastInteractable;
        
        private void Awake()
        {
            GetReferences();
            Init();
        }

        private void OnEnable() => AddListeners();

        private void OnDisable() => RemoveListeners();
        
        private void GetReferences()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Init()
        {
            _inputActionAsset = _playerInput.actions;
            _mainCamera = Camera.main;
        }

        private void AddListeners()
        {
            _inputActionAsset["Interact"].performed += Interact;
        }

        private void RemoveListeners()
        {
            _inputActionAsset["Interact"].performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Ray ray = _mainCamera.ScreenPointToRay(GetMousePosition());
            Physics.Raycast(ray.origin, ray.direction * interactableRayDistance, out RaycastHit hit);

            if (!hit.collider
                || !hit.collider.CompareTag(INTERACTABLE_TAG))
                return;
            
            if (_lastInteractable == null
                || _lastInteractable.transform != hit.collider.transform)
                _lastInteractable = hit.collider.GetComponent<InteractableObject>();
                
            _lastInteractable.onInteract?.Invoke();
        }

        private Vector3 GetMousePosition() => (Vector3) _inputActionAsset["MousePosition"].ReadValue<Vector2>();
    }
}
