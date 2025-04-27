using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace slc.NIGHTSWIM.Systems
{
    public class VolumeSlider : MonoBehaviour
    {
        [Header("UI References")]
        public Slider volumeSlider;
        public TextMeshProUGUI volumeLabel;
        public Toggle muteToggle;

        [Header("Settings")]
        public string exposedParameter = "MasterVolume";
        private float savedVolume = 1.0f;

        private void Start()
        {
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
            muteToggle.onValueChanged.AddListener(OnMuteToggled);

            LoadInitialVolume();
        }

        private void LoadInitialVolume()
        {
            if (AudioManager.Instance.TryGetVolume(exposedParameter, out float t_volume))
            {
                volumeSlider.value = t_volume;
                UpdateVolumeLabel(t_volume);
                muteToggle.isOn = Mathf.Approximately(t_volume, 0f);
            }
        }

        private void OnSliderValueChanged(float t_value)
        {
            if (!muteToggle.isOn)
            {
                SetVolumeAndSave(t_value);
                UpdateVolumeLabel(t_value);
            }
        }

        private void OnMuteToggled(bool t_isToggled)
        {
            if (t_isToggled)
            {
                savedVolume = volumeSlider.value;
                SetVolumeAndSave(0f);
                UpdateVolumeLabel(0f);
            }
            else
            {
                SetVolumeAndSave(savedVolume);
                volumeSlider.value = savedVolume;
                UpdateVolumeLabel(savedVolume);
            }
        }

        private void SetVolumeAndSave(float t_value)
        {
            AudioManager.Instance.SetVolume(exposedParameter, t_value);
        }

        private void UpdateVolumeLabel(float t_value)
        {
            int t_displayValue = Mathf.RoundToInt(t_value * 100.0f);
            volumeLabel.text = t_displayValue.ToString();
        }
    }
}