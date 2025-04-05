using UnityEngine;

namespace FaS.DiverGame
{
    public class CameraRecoilOnStroke : MonoBehaviour
    {
        [SerializeField] private float recoilAmount = 0.15f;
        [SerializeField] private float recoverySpeed = 4.0f;

        private Vector3 defaultPos;
        private Vector3 recoilOffset;
        private bool m_isRecoiling;

        private void Start()
        {
            defaultPos = transform.localPosition;
        }

        public void Activate()
        {
            recoilOffset = new Vector3(0f, -recoilAmount, -recoilAmount);
            m_isRecoiling = true;
        }

        private void Update()
        {
            if (m_isRecoiling)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos + recoilOffset, Time.deltaTime * 10.0f);
                if (Vector3.Distance(transform.localPosition, defaultPos + recoilOffset) < 0.01f)
                {
                    m_isRecoiling = false;
                }
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime * recoverySpeed);
            }
        }
    }
}