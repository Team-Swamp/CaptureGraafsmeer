using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Camera))]
    public sealed class FrustumCulling : MonoBehaviour
    {
        private const string CULL_ABLE_TAG = "Cullable";
        
        private GameObject[] _cullAbleObjects;
        private Renderer[] _cachedRenderers;
        private Camera _this;

        private void Awake() => _this = GetComponent<Camera>();

        private void Start() => InitCullingObjects();

        private void Update() => UpdateCullAbles();

        private void InitCullingObjects()
        {
            _cullAbleObjects = GameObject.FindGameObjectsWithTag(CULL_ABLE_TAG);
            _cachedRenderers = new Renderer[_cullAbleObjects.Length];
            int length = _cullAbleObjects.Length;
            
            for (int i = 0; i < length; i++)
            {
                _cachedRenderers[i] = _cullAbleObjects[i].GetComponent<Renderer>();
            }
        }

        private void UpdateCullAbles()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_this);
            int length = _cullAbleObjects.Length;
            
            for (int i = 0; i < length; i++)
            {
                bool isVisible = GeometryUtility.TestPlanesAABB(planes, _cachedRenderers[i].bounds);
                
                OcclusionCulling(ref isVisible, i);
                
                _cullAbleObjects[i].SetActive(isVisible);
            }
        }

        private void OcclusionCulling(ref bool isVisible, int i)
        {
            if (!isVisible) 
                return;
            
            Vector3 direction = _cullAbleObjects[i].transform.position - _this.transform.position;
            float distance = direction.magnitude;
            RaycastHit hit;
                
            if (Physics.Raycast(_this.transform.position, direction, out hit, distance)
                && hit.transform != _cullAbleObjects[i].transform)
                isVisible = false;
        }
    }
}