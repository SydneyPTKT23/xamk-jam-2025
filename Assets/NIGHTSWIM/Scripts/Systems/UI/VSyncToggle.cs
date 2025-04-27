using UnityEngine;
using UnityEngine.UI;

namespace slc.NIGHTSWIM.Systems
{
    public class VSyncToggle : MonoBehaviour
    {
        [Header("UI References")]
        public Toggle vSyncToggle;

        private void Start()
        {
            LoadCurrentVSync();
        }

        private void LoadCurrentVSync()
        {
            // If vSyncCount is greater than 0, VSync is ON
            vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        }

        public void OnVSyncToggleChanged(bool t_isOn)
        {
            QualitySettings.vSyncCount = t_isOn ? 1 : 0;
        }
    }
}