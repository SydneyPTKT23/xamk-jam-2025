using FaS.DiverGame.Input;
using UnityEngine;
using UnityEngine.UI;

namespace FaS.DiverGame.UI
{

    public class StaminaController : MonoBehaviour
    {
        public Slider staminaSlider;
        public InputHandler input;

        public bool usingStamina = false;

        public float staminaDecreaseSpeed = 1.0f;
        public float staminaRechargeSpeed = 1.0f;

        private void Update()
        {
            if (input.HasInputY)
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