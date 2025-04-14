using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    public class Floater : MonoBehaviour
    {
        public float degreesPerSecond = 15.0f;
        public float amplitude = 0.5f;
        public float frequency = 1f;

        private Vector3 posOffset = new();
        private Vector3 tempPos = new();

        private void Start()
        {
            // Store the starting position & rotation of the object
            posOffset = transform.position;
        }

        private void Update()
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }
}