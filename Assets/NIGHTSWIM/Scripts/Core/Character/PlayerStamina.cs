using UnityEngine;
using UnityEngine.Events;

namespace slc.NIGHTSWIM.Core
{
    public class PlayerStamina : MonoBehaviour
    {
        [Header("Stamina Settings")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float regenRate = 10f;
        [SerializeField] private float drainPerSwim = 20f;
        [SerializeField] private float regenDelay = 2f;

        [Header("Events")]
        public UnityEvent OnExhausted;
        public UnityEvent OnRecovered;

        public float CurrentStamina { get; private set; }
        public bool IsExhausted => CurrentStamina <= 0f;

        private float lastDrainTime;
        private bool wasExhausted;

        private void Start()
        {
            CurrentStamina = maxStamina;
        }

        private void Update()
        {
            RegenerateStamina();
        }

        public bool CanSwim()
        {
            return CurrentStamina >= drainPerSwim;
        }

        public void DrainStamina()
        {
            CurrentStamina = Mathf.Max(0f, CurrentStamina - drainPerSwim);
            lastDrainTime = Time.time;

            if (IsExhausted && !wasExhausted)
            {
                wasExhausted = true;
                OnExhausted?.Invoke();
            }
        }

        private void RegenerateStamina()
        {
            if (Time.time - lastDrainTime < regenDelay || IsExhausted)
                return;

            CurrentStamina = Mathf.Min(maxStamina, CurrentStamina + regenRate * Time.deltaTime);

            if (wasExhausted && CurrentStamina >= drainPerSwim)
            {
                wasExhausted = false;
                OnRecovered?.Invoke();
            }
        }

        public float GetStaminaNormalized() => CurrentStamina / maxStamina;
    }
}
