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
        public GameObject StartButton;

        public void ExitGame()
        {
            Application.Quit();
        }

        public void StartGame()
        {
            SceneManager.LoadScene(sceneName:"UIscene");


            GameObject.Find("StartButton").GetComponentInChildren<TextMeshProUGUI>().text = "keep going.";

        }
    }
}