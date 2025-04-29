using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace slc.NIGHTSWIM.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject pauseMenuUI;
        public GameObject settingsMenuUI;

        private bool isPaused = false;

        void Start()
        {
            pauseMenuUI.SetActive(false);
            settingsMenuUI?.SetActive(false);
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }

        public void PauseGame()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void ResumeGame()
        {
            pauseMenuUI.SetActive(false);
            settingsMenuUI?.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void OpenSettings()
        {
            //pauseMenuUI.SetActive(false);
            settingsMenuUI?.SetActive(true);
        }

        public void RestartCheckpoint()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // basic restart
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu"); // replace with your actual main menu scene name
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
