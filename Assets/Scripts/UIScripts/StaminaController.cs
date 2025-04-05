using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

namespace FaS.DiverGame.UI
{

    public class StaminaController : MonoBehaviour
    {
        public Slider staminaSlider;

        public bool usingStamina = false;

        public float staminaDecreaseSpeed = 1.0f;
        public float staminaRechargeSpeed = 1.0f;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (usingStamina)
            {
                DecreaseStamina();
            }
            else 
            { 
                RecharceStamina();
            }
        }

        public void DecreaseStamina()
        {
            staminaSlider.value -= 0.0001f * staminaDecreaseSpeed;
        }

        public void RecharceStamina()
        {
            staminaSlider.value += 0.0001f * staminaRechargeSpeed;
        }
    }


}