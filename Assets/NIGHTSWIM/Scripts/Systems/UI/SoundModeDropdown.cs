using TMPro;
using UnityEngine;

namespace slc.NIGHTSWIM.Systems
{
    public class SoundModeDropdown : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown soundModeDropdown;

        private void Start()
        {
            soundModeDropdown.onValueChanged.AddListener(OnSoundModeChanged);
        }

        private void OnSoundModeChanged(int t_index)
        {
            ApplySoundMode(t_index);
        }

        private void ApplySoundMode(int t_index)
        {
            Debug.Log("test");

            switch (t_index)
            {
                case 0:
                    SetStereoMode();
                    break;
                case 1:
                    SetMonoMode();
                    break;
                case 2:
                    SetSurroundSoundMode();
                    break;
                default:
                    SetStereoMode(); // Default to Stereo
                    break;
            }
        }

        private void SetStereoMode()
        {
            AudioConfiguration t_config = AudioSettings.GetConfiguration();
            t_config.speakerMode = AudioSpeakerMode.Stereo;
            AudioSettings.Reset(t_config);
        }

        private void SetMonoMode()
        {
            AudioConfiguration t_config = AudioSettings.GetConfiguration();
            t_config.speakerMode = AudioSpeakerMode.Mono;
            AudioSettings.Reset(t_config);
        }

        private void SetSurroundSoundMode()
        {
            AudioConfiguration t_config = AudioSettings.GetConfiguration();
            t_config.speakerMode = AudioSpeakerMode.Surround;
            AudioSettings.Reset(t_config);
        }
    }
}