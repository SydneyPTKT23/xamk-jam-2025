using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace slc.NIGHTSWIM.Systems
{
    public class DisplayModeDropdown : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown displayModeDropdown;

        private void Start()
        {
            PopulateDisplayModeDropdown();
            LoadCurrentDisplayMode();
        }

        private void PopulateDisplayModeDropdown()
        {
            // Clear existing options
            displayModeDropdown.ClearOptions();

            // Add display mode options to the dropdown
            List<string> t_displayModeOptions = new()
            {
                "Fullscreen",
                "Windowed",
                "Borderless"
            };

            displayModeDropdown.AddOptions(t_displayModeOptions);
            displayModeDropdown.RefreshShownValue();
        }

        private void LoadCurrentDisplayMode()
        {
            // Get the current display mode
            FullScreenMode t_currentMode = Screen.fullScreenMode;
            switch (t_currentMode)
            {
                case FullScreenMode.FullScreenWindow:
                    displayModeDropdown.value = 0;
                    break;

                case FullScreenMode.Windowed:
                    displayModeDropdown.value = 1;
                    break;

                case FullScreenMode.MaximizedWindow:
                    displayModeDropdown.value = 2;
                    break;
            }
        }

        public void OnDisplayModeChanged(int t_index)
        {
            switch (t_index)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;

                case 1:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;

                case 2:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
            }
        }
    }
}