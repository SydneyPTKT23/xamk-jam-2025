using UnityEngine;
using UnityEngine.Audio;

namespace slc.NIGHTSWIM.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixer[] m_audioMixers;

        /// <summary>
        /// Finds matching audio groups by sub-path in all audio mixers.
        /// </summary>
        public AudioMixerGroup[] FindMatchingGroups(string t_subPath)
        {
            if (m_audioMixers == null || m_audioMixers.Length == 0)
                return null;

            foreach (AudioMixer t_mixer in m_audioMixers)
            {
                if (t_mixer != null)
                {
                    AudioMixerGroup[] t_results = t_mixer.FindMatchingGroups(t_subPath);
                    if (t_results != null && t_results.Length > 0)
                    {
                        return t_results;
                    }
                }
            }

            return null;
        }

        public void SetFloat(string t_name, float t_value)
        {
            if (m_audioMixers == null || m_audioMixers.Length == 0)
                return;

            foreach (AudioMixer t_mixer in m_audioMixers)
            {
                if (t_mixer != null)
                {
                    t_mixer.SetFloat(t_name, t_value);
                    break; // Exit early after setting for the first valid mixer
                }
            }
        }

        public void GetFloat(string t_name, out float t_value)
        {
            t_value = 0f;

            if (m_audioMixers == null || m_audioMixers.Length == 0)
                return;

            foreach (AudioMixer t_mixer in m_audioMixers)
            {
                if (t_mixer != null)
                {
                    t_mixer.GetFloat(t_name, out t_value);
                    return; // Exit early after getting the value from the first valid mixer
                }
            }
        }
    }
}
