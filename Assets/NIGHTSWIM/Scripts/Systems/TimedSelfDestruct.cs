using UnityEngine;

namespace slc.NIGHTSWIM.Systems
{
    public class TimedSelfDestruct : MonoBehaviour
    {
        public float lifeTime = 1.0f;
        float m_spawnTime;

        private void Awake()
        {
            m_spawnTime = Time.time;
        }

        private void Update()
        {
            if (Time.time > m_spawnTime + lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}