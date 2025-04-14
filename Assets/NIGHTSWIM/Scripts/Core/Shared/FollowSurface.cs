using slc.NIGHTSWIM.WaterSystem;
using UnityEngine;

namespace slc.NIGHTSWIM
{
    public class FollowSurface : MonoBehaviour
    {
        [SerializeField] protected WaterSurfaceController waterSurfaceController;

        public float floatHeight = 0.2f;
        public float followSpeed = 5.0f;

        private float targetHeight;

        private void Update()
        {
            Float(transform);
        }

        public void Float(Transform t_transform)
        {
            // Get the current height of the water at the object's position
            float t_waterHeight = waterSurfaceController.GetHeight(t_transform.position);

            // Calculate the target height based on the water level and the desired float height
            targetHeight = t_waterHeight + floatHeight;

            // Smoothly move the object towards the target height to prevent bouncing
            Vector3 t_newPosition = t_transform.position;
            t_newPosition.y = Mathf.Lerp(t_newPosition.y, targetHeight, followSpeed * Time.deltaTime);

            // Apply the new position to the transform
            t_transform.position = t_newPosition;
        }
    }
}
