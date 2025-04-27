using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace slc.NIGHTSWIM.Systems
{
    public class FramerateCapDropdown : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown framerateDropdown;

        private readonly int[] m_framerateOptions = { 30, 60, 120, 144, 240, -1 }; // -1 = Unlimited

        private void Start()
        {
            PopulateDropdown();
            LoadCurrentFramerateCap();
        }

        private void PopulateDropdown()
        {
            framerateDropdown.ClearOptions();
            List<string> t_options = new();

            foreach (int t_framerate in m_framerateOptions)
            {
                t_options.Add(t_framerate == -1 ? "Unlimited" : t_framerate + " FPS");
            }

            framerateDropdown.AddOptions(t_options);
        }

        private void LoadCurrentFramerateCap()
        {
            int t_currentCap = Application.targetFrameRate;
            int t_selectedIndex = System.Array.IndexOf(m_framerateOptions, t_currentCap);

            if (t_selectedIndex == -1)
            {
                // Default to 60 FPS if current setting doesn't match
                t_selectedIndex = 1;
            }

            framerateDropdown.SetValueWithoutNotify(t_selectedIndex);
        }

        public void OnFramerateDropdownChanged(int t_index)
        {
            int t_selectedCap = m_framerateOptions[t_index];
            Application.targetFrameRate = t_selectedCap;
        }
    }
}