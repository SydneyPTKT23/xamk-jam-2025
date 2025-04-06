using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FaS.DiverGame.UI
{

    public class MenuController : MonoBehaviour
    {
        public GameObject PauseMenu;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu.SetActive(true);
            } 
        }


        public void ExitGame()
        {
            Application.Quit();
        }

        public void StartGame()
        {
        SceneManager.LoadScene(sceneName:"DebugScene");


            GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "CHICKEN JOCKEY.";
            GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "hi";
            //StartCoroutine(waitTime());
            //GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "keep going.";

        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(sceneName: "MainMenu");
            //GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "keep going.";
        }

        public void HidePauseMenu()
        {
            PauseMenu.SetActive(false);
        }

        /*
        IEnumerator waitTime()
        {
            yield return new WaitForSeconds(3);
        }

        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }*/
    }
}