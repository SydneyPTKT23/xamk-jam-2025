using UnityEngine;

namespace FaS.DiverGame
{
    public class CameraFOVBurst : MonoBehaviour
    {
        [SerializeField] private float defaultFOV = 60.0f;
        [SerializeField] private float burstFOV = 70.0f;
        [SerializeField] private float fovSpeed = 5.0f;

        private float m_targetFOV;
        private Camera m_cam;

        private void Start()
        {
            m_cam = GetComponentInChildren<Camera>();
            m_targetFOV = defaultFOV;
            m_cam.fieldOfView = defaultFOV;
        }

        public void Activate()
        {
            m_targetFOV = burstFOV;
        }

        private void Update()
        {
            m_cam.fieldOfView = Mathf.Lerp(m_cam.fieldOfView, m_targetFOV, Time.deltaTime * fovSpeed);
            if (Mathf.Abs(m_cam.fieldOfView - burstFOV) < 0.5f)
                m_targetFOV = defaultFOV;
        }
    }
}