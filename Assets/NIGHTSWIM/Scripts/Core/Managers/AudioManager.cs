using UnityEngine;
using UnityEngine.Audio;

namespace slc.NIGHTSWIM.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Mixer Reference")]
        public AudioMixer m_audioMixer;

        [Header("Volume Settings")]
        public float minDecibels = -80.0f; // Silence in dB
        public float maxDecibels = 0f; // Full volume in dB

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Finds matching audio groups by sub-path in the audio mixer.
        /// </summary>
        public AudioMixerGroup[] FindMatchingGroups(string t_subPath)
        {
            if (m_audioMixer == null)
                return null;

            AudioMixerGroup[] t_results = m_audioMixer.FindMatchingGroups(t_subPath);
            return t_results != null && t_results.Length > 0 ? t_results : null;
        }

        public void SetVolume(string t_parameterName, float t_normalizedValue)
        {
            t_normalizedValue = Mathf.Clamp(t_normalizedValue, 0f, 1.0f);

            // Apply the volume to the audio mixer
            float dB = Mathf.Lerp(minDecibels, maxDecibels, t_normalizedValue);
            _ = m_audioMixer.SetFloat(t_parameterName, dB);
        }

        public bool TryGetVolume(string t_parameterName, out float t_normalizedValue)
        {
            if (m_audioMixer.GetFloat(t_parameterName, out float dB))
            {
                t_normalizedValue = Mathf.InverseLerp(minDecibels, maxDecibels, dB);
                return true;
            }

            t_normalizedValue = 1.0f; // Default to full if unknown
            return false;
        }
    }
}