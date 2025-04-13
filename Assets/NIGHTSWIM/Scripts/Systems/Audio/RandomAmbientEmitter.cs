using UnityEngine;
using System.Collections;

namespace slc.NIGHTSWIM.Systems
{
    public class RandomAmbientEmitter : MonoBehaviour
    {
        [Header("Monster Ambience")]
        public AudioClip[] m_ambientClips;
        public float m_minDelay = 8f;
        public float m_maxDelay = 20f;

        [Header("Distance Range From Player")]
        public float m_minSpawnDistance = 25f;
        public float m_maxSpawnDistance = 50f;

        [Header("Audio Settings")]
        public float m_spatialBlend = 1f; // Full 3D
        public float m_rolloffMin = 10f;
        public float m_rolloffMax = 60f;

        private Transform m_player;
        private AudioClip m_lastClip;
        private int m_clipCount;

        private void Start()
        {
            m_player = Camera.main != null ? Camera.main.transform : null;
            if (m_player == null)
            {
                Debug.LogWarning("RandomAmbientEmitter: No player (Camera.main) found.");
                enabled = false;
                return;
            }

            m_clipCount = m_ambientClips.Length;
            if (m_clipCount < 1)
            {
                Debug.LogWarning("RandomAmbientEmitter: No audio clips assigned.");
                enabled = false;
                return;
            }

            _ = StartCoroutine(PlayRandomDistantAmbience());
        }

        private IEnumerator PlayRandomDistantAmbience()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(Random.Range(m_minDelay, m_maxDelay));

                if (m_clipCount < 2 || m_player == null)
                    continue;

                AudioClip t_clip = GetNextClip();
                Vector3 t_position = GetRandomDistantPosition();

                AudioUtility.CreateSFX(
                    t_clip,
                    t_position,
                    AudioUtility.AudioGroups.Ambient,
                    m_spatialBlend,
                    m_rolloffMin,
                    m_rolloffMax
                );
            }
        }

        private AudioClip GetNextClip()
        {
            AudioClip t_newClip;
            int t_attempts = 0;

            do
            {
                t_newClip = m_ambientClips[Random.Range(0, m_clipCount)];
                t_attempts++;
            }
            while (t_newClip == m_lastClip && t_attempts < 10);

            m_lastClip = t_newClip;
            return t_newClip;
        }

        private Vector3 GetRandomDistantPosition()
        {
            Vector2 t_circle = Random.insideUnitCircle.normalized * Random.Range(m_minSpawnDistance, m_maxSpawnDistance);
            Vector3 t_offset = new(t_circle.x, 0f, t_circle.y);
            Vector3 t_position = m_player.position + t_offset;
            t_position.y = m_player.position.y; // Water level
            return t_position;
        }
    }
}