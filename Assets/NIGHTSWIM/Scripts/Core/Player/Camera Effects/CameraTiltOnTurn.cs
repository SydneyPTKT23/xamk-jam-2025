using slc.NIGHTSWIM.Input;
using UnityEngine;

namespace slc.NIGHTSWIM
{
    public class CameraTiltOnTurn : MonoBehaviour
    {
        [SerializeField] private Transform m_cameraRoot;
        [SerializeField] private float maxTilt = 10.0f;
        [SerializeField] private float tiltSmoothSpeed = 5.0f;

        private float m_currentTilt = 0f;

        private InputHandler m_inputHandler;

        private void Start()
        {
            m_inputHandler = GetComponentInParent<InputHandler>();
        }

        private void Update()
        {
            float t_turnInput = m_inputHandler.InputVector.x;
            float t_targetTilt = -t_turnInput * maxTilt;

            m_currentTilt = Mathf.Lerp(m_currentTilt, t_targetTilt, Time.deltaTime * tiltSmoothSpeed);
            m_cameraRoot.localRotation = Quaternion.Euler(0f, 0f, m_currentTilt);
        }
    }
}