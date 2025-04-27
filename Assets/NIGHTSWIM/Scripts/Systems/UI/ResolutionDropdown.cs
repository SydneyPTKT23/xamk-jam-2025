using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace slc.NIGHTSWIM.Systems
{
    public class ResolutionDropdown : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown resolutionDropdown;  // Reference to the TMP Dropdown
        public TextMeshProUGUI resolutionLabel;   // Label to show current resolution (optional)
        public Scrollbar scrollbar;               // Scrollbar reference for scrolling to selected item

        private Resolution[] availableResolutions;
        private Resolution[] sortedResolutions;    // Sorted resolutions list
        private int currentResolutionIndex = 0;

        private void Start()
        {
            PopulateResolutionDropdown();
            LoadCurrentResolution();
        }

        private void PopulateResolutionDropdown()
        {
            // Get available resolutions from the system
            availableResolutions = Screen.resolutions;

            // Filter out duplicate resolutions (same width & height, only keep unique)
            var uniqueResolutions = availableResolutions
                .GroupBy(res => new { res.width, res.height })  // Group by width and height
                .Select(group => group.First())                 // Take the first one from each group
                .ToArray();

            // Sort resolutions from largest to smallest (by width and height)
            sortedResolutions = uniqueResolutions
                .OrderByDescending(res => res.width * res.height)  // Sort by total area (width * height)
                .ThenByDescending(res => res.width) // Then by width in case of a tie
                .ToArray();

            // Clear existing options in case we're updating the list
            resolutionDropdown.ClearOptions();

            // Create a list to hold the resolution names
            var resolutionOptions = new System.Collections.Generic.List<string>();

            // Loop through the sorted resolutions and add them to the dropdown list
            foreach (var resolution in sortedResolutions)
            {
                string resolutionString = $"{resolution.width}x{resolution.height}";
                resolutionOptions.Add(resolutionString);
            }

            // Add the sorted resolution options to the dropdown
            resolutionDropdown.AddOptions(resolutionOptions);

            // Ensure the dropdown reflects the updated options
            resolutionDropdown.RefreshShownValue();
        }

        private void LoadCurrentResolution()
        {
            // Get the current screen resolution
            Resolution currentResolution = Screen.currentResolution;

            // Debug log to verify current resolution
            Debug.Log($"Current Resolution: {currentResolution.width}x{currentResolution.height}");

            // Find the index of the current resolution in the sorted resolutions
            for (int i = 0; i < sortedResolutions.Length; i++)
            {
                if (sortedResolutions[i].width == currentResolution.width &&
                    sortedResolutions[i].height == currentResolution.height)
                {
                    currentResolutionIndex = i;
                    break;
                }
            }

            // Set the dropdown to show the correct resolution
            resolutionDropdown.value = currentResolutionIndex;

            // Ensure the dropdown updates its displayed value
            resolutionDropdown.RefreshShownValue();

            // Scroll to the selected resolution
            if (scrollbar != null)
            {
                GoToSelected();
            }
        }

        public void OnResolutionChanged(int index)
        {
            // Apply the selected resolution
            Resolution selectedResolution = sortedResolutions[index];

            // Set the resolution
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);

            // Optionally, update the label to reflect the selected resolution
            resolutionLabel.text = $"{selectedResolution.width}x{selectedResolution.height}";
        }

        // This function is called when the dropdown is opened
        public void OnDropdownValueChanged(int t_index)
        {       
            // Scroll to the selected option when the dropdown is opened
            if (scrollbar != null)
            {
                GoToSelected();
            }
        }

        private void GoToSelected()
        {
            Debug.Log("sus");

            float selectedOption = resolutionDropdown.value;
            float ratio = selectedOption / resolutionDropdown.options.Count;

            scrollbar.value = 1f - ratio;
        }
    }
}