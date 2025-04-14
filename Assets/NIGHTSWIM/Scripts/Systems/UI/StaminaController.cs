using slc.NIGHTSWIM.Core;
using UnityEngine;
using UnityEngine.UI;

namespace slc.NIGHTSWIM.UI
{
    [RequireComponent(typeof(Slider))]
    public class StaminaController : MonoBehaviour
    {
        [Header("References")]
        public PlayerStamina playerStamina;
        public Slider staminaSlider;

        private void Start()
        {
            if (staminaSlider == null)
                staminaSlider = GetComponent<Slider>();

            if (playerStamina == null)
                playerStamina = FindObjectOfType<PlayerStamina>();

            staminaSlider.minValue = 0f;
            staminaSlider.maxValue = 1f;
        }

        private void Update()
        {
            if (playerStamina != null)
            {
                staminaSlider.value = playerStamina.GetStaminaNormalized();
            }
        }
    }
}