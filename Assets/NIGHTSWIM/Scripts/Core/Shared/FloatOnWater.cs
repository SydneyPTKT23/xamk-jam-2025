using UnityEngine;
using slc.NIGHTSWIM.WaterSystem;

namespace slc.NIGHTSWIM.Core
{
    public class FloatOnWater : MonoBehaviour
    {
        public WaterSurfaceManager waterSurface;
        public float floatOffset = 0.5f;
        public float floatSmoothSpeed = 2f;
        public bool alignToSurface = false;

        private void Update()
        {
            Vector3 pos = transform.position;

            // Get water height at current position
            float waterHeight = waterSurface.GetHeightAtWorldPosition(pos);
            float targetY = waterHeight + floatOffset;
            float smoothedY = Mathf.Lerp(pos.y, targetY, Time.deltaTime * floatSmoothSpeed);
            transform.position = new Vector3(pos.x, smoothedY, pos.z);

            if (alignToSurface)
            {
                float delta = 0.25f; // balanced sample distance; adjust for your wave scale

                // Get surrounding heights
                float hL = waterSurface.GetHeightAtWorldPosition(new Vector3(pos.x - delta, pos.y, pos.z));
                float hR = waterSurface.GetHeightAtWorldPosition(new Vector3(pos.x + delta, pos.y, pos.z));
                float hD = waterSurface.GetHeightAtWorldPosition(new Vector3(pos.x, pos.y, pos.z - delta));
                float hU = waterSurface.GetHeightAtWorldPosition(new Vector3(pos.x, pos.y, pos.z + delta));

                // Compute surface normal instantly
                Vector3 normal = new Vector3(hL - hR, 2f * delta, hD - hU).normalized;

                // Keep forward direction consistent, project it onto the surface plane
                Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, normal).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(projectedForward, normal);

                // Apply rotation smoothing only — keep it responsive but still smooth
                float t_smooth = Mathf.Clamp01(Time.deltaTime * floatSmoothSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t_smooth);
            }
        }
    }
}