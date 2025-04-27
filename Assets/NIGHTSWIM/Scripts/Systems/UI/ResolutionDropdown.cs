using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

namespace slc.NIGHTSWIM.Systems
{
    public class ResolutionDropdown : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown resolutionDropdown;

        private Resolution[] m_availableResolutions;
        private Resolution[] m_sortedResolutions;
        private int m_currentResolutionIndex = 0;

        private void Start()
        {
            PopulateResolutionDropdown();
            LoadCurrentResolution();
        }

        private void PopulateResolutionDropdown()
        {
            // Get available resolutions from the system
            m_availableResolutions = Screen.resolutions;

            // Filter out duplicate resolutions (same width & height, only keep unique)
            Resolution[] t_uniqueResolutions = m_availableResolutions
                .GroupBy(res => new { res.width, res.height })  // Group by width and height
                .Select(group => group.First())                 // Take the first one from each group
                .ToArray();

            // Sort resolutions from largest to smallest (by width and height)
            m_sortedResolutions = t_uniqueResolutions
                .OrderByDescending(res => res.width * res.height)  // Sort by total area (width * height)
                .ThenByDescending(res => res.width) // Then by width in case of a tie
                .ToArray();

            // Clear existing options in case we're updating the list
            resolutionDropdown.ClearOptions();

            // Create a list to hold the resolution names
            List<string> t_resolutionOptions = new();

            // Loop through the sorted resolutions and add them to the dropdown list
            foreach (var t_resolution in m_sortedResolutions)
            {
                string t_resolutionString = $"{t_resolution.width}x{t_resolution.height}";
                t_resolutionOptions.Add(t_resolutionString);
            }

            // Add the sorted resolution options to the dropdown
            resolutionDropdown.AddOptions(t_resolutionOptions);

            // Ensure the dropdown reflects the updated options
            resolutionDropdown.RefreshShownValue();
        }

        private void LoadCurrentResolution()
        {
            // Get the current screen resolution
            Resolution t_currentResolution = Screen.currentResolution;
            Debug.Log($"Current Resolution: {t_currentResolution.width}x{t_currentResolution.height}");

            // Find the index of the current resolution in the sorted resolutions
            for (int i = 0; i < m_sortedResolutions.Length; i++)
            {
                if (m_sortedResolutions[i].width == t_currentResolution.width &&
                    m_sortedResolutions[i].height == t_currentResolution.height)
                {
                    m_currentResolutionIndex = i;
                    break;
                }
            }

            // Set the dropdown to show the correct resolution
            resolutionDropdown.value = m_currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void OnResolutionChanged(int t_index)
        {
            Resolution t_selectedResolution = m_sortedResolutions[t_index];
            Screen.SetResolution(t_selectedResolution.width, t_selectedResolution.height, Screen.fullScreenMode);
        }
    }
}