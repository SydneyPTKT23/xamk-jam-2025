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

        [Tooltip("Distance between sample points for normal calculation")]
        public float sampleDistance = 1f;

        private void Update()
        {
            Vector3 pos = transform.position;

            // Smooth floating
            float waterHeight = waterSurface.GetHeightAtWorldPosition(pos);
            float targetY = waterHeight + floatOffset;
            float smoothedY = Mathf.Lerp(pos.y, targetY, Time.deltaTime * floatSmoothSpeed);
            transform.position = new Vector3(pos.x, smoothedY, pos.z);

            if (alignToSurface)
            {
                // Sample in fixed world-space directions to avoid feedback loop
                Vector3 sampleForward = pos + Vector3.forward * sampleDistance;
                Vector3 sampleRight = pos + Vector3.right * sampleDistance;

                float heightForward = waterSurface.GetHeightAtWorldPosition(sampleForward);
                float heightRight = waterSurface.GetHeightAtWorldPosition(sampleRight);

                sampleForward.y = heightForward;
                sampleRight.y = heightRight;
                pos.y = waterHeight;

                Vector3 toForward = sampleForward - pos;
                Vector3 toRight = sampleRight - pos;

                Vector3 normal = Vector3.Cross(toRight, toForward).normalized;

                if (normal.sqrMagnitude > 0.0001f)
                {
                    // Align object's up with water surface normal, but preserve yaw
                    Quaternion targetRotation = Quaternion.LookRotation(
                        Vector3.ProjectOnPlane(transform.forward, normal),
                        normal
                    );

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * floatSmoothSpeed);
                }
            }
        }
    }
}
