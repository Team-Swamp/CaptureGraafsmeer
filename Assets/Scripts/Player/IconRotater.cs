using UnityEngine;

namespace Player
{
    public sealed class IconRotater : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;

        private void Update()
        {
            Quaternion targetRotation = targetTransform.rotation;
            Quaternion newYRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = newYRotation;
        }
    }
}
