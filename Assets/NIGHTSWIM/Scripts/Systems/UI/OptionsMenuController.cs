using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace slc.NIGHTSWIM
{
    public class OptionsMenuController : MonoBehaviour
    {
        [Header("Category Panels")]
        public GameObject displayPanel;
        public GameObject graphicsPanel;
        public GameObject audioPanel;
        public GameObject controlsPanel;

        private void Start()
        {
            OpenDisplay(); // Default to display panel
        }

        private void DisableAllPanels()
        {
            displayPanel.SetActive(false);
            graphicsPanel.SetActive(false);
            audioPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        public void OpenDisplay()
        {
            DisableAllPanels();
            displayPanel.SetActive(true);
        }

        public void OpenGraphics()
        {
            DisableAllPanels();
            graphicsPanel.SetActive(true);
        }

        public void OpenAudio()
        {
            DisableAllPanels();
            audioPanel.SetActive(true);
        }

        public void OpenControls()
        {
            DisableAllPanels();
            controlsPanel.SetActive(true);
        }
    }
}
