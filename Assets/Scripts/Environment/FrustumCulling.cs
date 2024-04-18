using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Camera))]
    public sealed class FrustumCulling : MonoBehaviour
    {
        private const string CULL_ABLE_TAG = "CullAble";
        
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
                _cullAbleObjects[i].SetActive(GeometryUtility.TestPlanesAABB(planes, _cachedRenderers[i].bounds));
            }
        }
    }
}