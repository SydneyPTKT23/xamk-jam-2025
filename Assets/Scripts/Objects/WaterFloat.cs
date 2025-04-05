using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaS.DiverGame
{
    public class WaterFloat : MonoBehaviour
    {
        public float floatOffset = 0.5f;

        public float floatSmoothSpeed = 2f;

        public bool alignToSurface = false;

        private void Update()
        {
            if (Terrain.SimplePlane.Instance == null)
                return;

            Vector3 pos = transform.position;

            // Get water height at current position
            float waterHeight = Terrain.SimplePlane.Instance.GetHeightAtWorldPosition(pos);
            float targetY = waterHeight + floatOffset;
            float smoothedY = Mathf.Lerp(pos.y, targetY, Time.deltaTime * floatSmoothSpeed);
            transform.position = new Vector3(pos.x, smoothedY, pos.z);

            if (alignToSurface)
            {
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;
                float sampleOffset = 0.5f;

                // Sample heights in local X and Z directions
                Vector3 pointForward = pos + forward * sampleOffset;
                Vector3 pointRight = pos + right * sampleOffset;

                float heightForward = Terrain.SimplePlane.Instance.GetHeightAtWorldPosition(pointForward);
                float heightRight = Terrain.SimplePlane.Instance.GetHeightAtWorldPosition(pointRight);

                // Build direction vectors
                Vector3 tangentZ = new(0f, heightForward - waterHeight, sampleOffset);
                Vector3 tangentX = new(sampleOffset, heightRight - waterHeight, 0f);

                // Calculate normal using cross product
                Vector3 normal = Vector3.Cross(tangentZ, tangentX).normalized;

                // Smoothly rotate toward the surface normal
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * floatSmoothSpeed);
            }
        }
    }
}