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
        [SerializeField] private float recoveryThreshold = 20f; // Amount of stamina required to recover

        [Header("Events")]
        public UnityEvent OnExhausted;
        public UnityEvent OnRecovered;

        public float CurrentStamina { get; private set; }
        public bool IsExhausted => CurrentStamina <= 0f;
        public bool IsHardExhausted { get; private set; }

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

        public void RegenerateStamina()
        {
            if (Time.time - lastDrainTime < regenDelay) return;

            CurrentStamina = Mathf.Min(maxStamina, CurrentStamina + regenRate * Time.deltaTime);

            if (IsHardExhausted && CurrentStamina >= recoveryThreshold)
            {
                RecoverFromExhaustion();
            }

            if (wasExhausted && CurrentStamina > 0f)
            {
                wasExhausted = false;
                OnRecovered?.Invoke();
            }
        }

        public void EnterExhaustion()
        {
            IsHardExhausted = true;
            OnExhausted?.Invoke();
        }

        private void RecoverFromExhaustion()
        {
            IsHardExhausted = false;
            OnRecovered?.Invoke();
        }

        public bool CanSwim()
        {
            return !IsHardExhausted;
        }

        public float GetStaminaNormalized() => CurrentStamina / maxStamina;
    }
}
