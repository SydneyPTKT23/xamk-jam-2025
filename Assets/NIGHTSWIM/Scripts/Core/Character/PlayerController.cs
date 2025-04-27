using slc.NIGHTSWIM.Input;
using slc.NIGHTSWIM.WaterSystem;
using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    [RequireComponent(typeof(Rigidbody), typeof(InputManager))]
    public class PlayerController : MonoBehaviour
    {
        #region Inspector Variables

        [Header("References")]
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private WaterSurfaceController m_surfaceController;
        [SerializeField] private AudioClip strokeSfx;

        [Header("Movement Settings")]
        [SerializeField] private float passiveSwimSpeed = 2.0f;
        [SerializeField] private float strokeForce = 5.0f;
        [SerializeField] private float strokeCooldown = 0.6f;

        [Header("Stroke Boost")]
        [SerializeField] private float boostDuration = 0.3f;
        [SerializeField] private AnimationCurve boostCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        [Header("Turning")]
        [SerializeField] private float turnSpeed = 90.0f;

        [Header("Floating")]
        public float floatHeight = 1.0f;
        public float followSpeed = 5.0f;

        #endregion

        #region Private Variables
        private Rigidbody m_rb;
        private InputManager m_inputManager;
        private PlayerAnimationController m_animationController;
        private CameraController m_cameraController;
        private PlayerStamina m_stamina;

        private Vector3 currentDirection = Vector3.forward;
        private bool isStroking = false;
        private bool isBoosting = false;

        private float strokeTimer = 0f;
        private float lastStrokeTime = Mathf.NegativeInfinity;
        private float boostTimer = 0f;
        #endregion

        #region Built-In Methods
        private void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.useGravity = false;

            m_inputManager = GetComponent<InputManager>();
            m_animationController = GetComponent<PlayerAnimationController>();
            m_cameraController = GetComponentInChildren<CameraController>();
            m_stamina = GetComponent<PlayerStamina>();
        }

        private void Update()
        {
            RotatePlayer();
            HandleStrokeInput();
            FollowSurface();
        }

        private void FixedUpdate()
        {
            HandlePassiveSwim();
            HandleBoost();
        }
        #endregion

        #region Public Methods
        public void FollowSurface()
        {
            float t_waterHeight = m_surfaceController.GetHeight(transform.position);
            float t_targetHeight = t_waterHeight + floatHeight;

            Vector3 t_newPosition = transform.position;
            t_newPosition.y = Mathf.Lerp(t_newPosition.y, t_targetHeight, followSpeed * Time.deltaTime);
            m_rb.MovePosition(t_newPosition);
        }
        #endregion

        #region Private Methods
        private void RotatePlayer()
        {
            float t_yawInput = m_inputManager.InputVector.x;
            float t_yawRotation = t_yawInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, t_yawRotation, 0f);
        }

        private void HandleStrokeInput()
        {
            if (CanStroke() && m_inputManager.HasInputY)
            {
                StartStroke();
            }

            if (isStroking)
            {
                strokeTimer += Time.deltaTime;
                if (strokeTimer >= strokeCooldown)
                {
                    isStroking = false;
                }
            }
        }

        private void StartStroke()
        {
            m_cameraController.TriggerEffects();
            m_audioSource.PlayOneShot(strokeSfx);
            m_stamina.DrainStamina();

            strokeTimer = 0f;
            lastStrokeTime = Time.time;
            isStroking = true;

            currentDirection = m_inputManager.InputVector.y > 0 ? transform.forward : -transform.forward;
            m_rb.AddForce(currentDirection * strokeForce, ForceMode.Impulse);

            isBoosting = true;
            boostTimer = 0f;
            m_rb.AddForce(Vector3.up * 1.5f, ForceMode.Impulse);

            m_animationController.SetMoveTrigger();
        }

        private void HandlePassiveSwim()
        {
            if (m_inputManager.HasInputY && m_stamina.CanSwim())
            {
                Vector3 t_desiredDirection = m_inputManager.InputVector.y > 0 ? transform.forward : -transform.forward;
                Vector3 t_horizontalVelocity = m_rb.velocity;
                t_horizontalVelocity.y = 0f;

                if (t_horizontalVelocity.magnitude < passiveSwimSpeed)
                {
                    m_rb.AddForce(t_desiredDirection * passiveSwimSpeed, ForceMode.Force);
                }
            }
        }

        private void HandleBoost()
        {
            if (isBoosting)
            {
                boostTimer += Time.fixedDeltaTime;
                float t_normalizedTime = Mathf.Clamp01(boostTimer / boostDuration);

                if (t_normalizedTime < 1f)
                {
                    float t_curveValue = boostCurve.Evaluate(t_normalizedTime);
                    m_rb.AddForce(passiveSwimSpeed * t_curveValue * currentDirection, ForceMode.Force);
                }
                else
                {
                    isBoosting = false;
                }
            }
        }

        private bool CanStroke()
        {
            return !isStroking
                && (Time.time - lastStrokeTime) >= strokeCooldown
                && m_stamina.CanSwim();
        }
        #endregion
    }
}