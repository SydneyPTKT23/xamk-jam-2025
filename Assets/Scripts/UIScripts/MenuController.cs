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


        public void ExitGame()
        {
            Application.Quit();
        }

        public void StartGame()
        {
        SceneManager.LoadScene(sceneName:"DebugScene");


            GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "CHICKEN JOCKEY.";
            //StartCoroutine(waitTime());
            //GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "keep going.";

        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(sceneName: "MainMenu");
            GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "keep going.";
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