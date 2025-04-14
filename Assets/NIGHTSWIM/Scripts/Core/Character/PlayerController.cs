using slc.NIGHTSWIM.Input;
using slc.NIGHTSWIM.WaterSystem;
using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    [RequireComponent(typeof(CharacterController), typeof(InputManager))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public AudioSource audioSource;
        public WaterSurfaceController ctrl;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float moveDuration = 0.6f;
        [SerializeField] private float moveCooldown = 0.5f;
        [SerializeField] private AnimationCurve swimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Turning")]
        [SerializeField] private float turnSpeed = 90f;

        [Header("Audio")]
        public AudioClip strokeSfx;

        [Header("Floating")]
        public float floatHeight = 1f;

        [Header("Interaction")]
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactableCheckDistance = 2f;

        private CharacterController characterController;
        private InputManager inputManager;
        private PlayerAnimationController animationController;
        private CameraController cameraController;
        private PlayerStamina stamina;

        private Vector3 currentDirection;
        private bool isMoving = false;
        private float moveTimer = 0f;
        private float lastMoveTime = Mathf.NegativeInfinity;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            inputManager = GetComponent<InputManager>();
            animationController = GetComponent<PlayerAnimationController>();
            cameraController = GetComponentInChildren<CameraController>();
            stamina = GetComponent<PlayerStamina>();

            currentDirection = transform.forward;

            if (swimCurve == null || swimCurve.length == 0)
            {
                swimCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            }
        }

        private void Update()
        {
            RotatePlayer();
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (CanMove() && inputManager.HasInputY)
            {
                StartMovement();
            }

            if (!isMoving) return;

            moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(moveTimer / moveDuration);
            float speed = moveSpeed * swimCurve.Evaluate(t);
            float distanceThisFrame = speed * Time.deltaTime;

            // Prevent overshooting interactables
            if (Physics.Raycast(transform.position, currentDirection, out RaycastHit hit, interactableCheckDistance, interactableLayer))
            {
                if (hit.distance < distanceThisFrame)
                {
                    // Only move up to just before the object
                    distanceThisFrame = Mathf.Max(0f, hit.distance - 0.1f); // 0.1f safety margin
                    isMoving = false;
                }
            }

            characterController.Move(currentDirection * distanceThisFrame);

            if (moveTimer >= moveDuration)
            {
                isMoving = false;
            }
        }

        private void StartMovement()
        {
            cameraController.TriggerEffects();
            audioSource.PlayOneShot(strokeSfx);

            stamina.DrainStamina();

            moveTimer = 0f;
            lastMoveTime = Time.time;
            isMoving = true;

            currentDirection = inputManager.InputVector.y > 0 ? transform.forward : -transform.forward;
            animationController.SetMoveTrigger();
        }

        private void RotatePlayer()
        {
            float yawInput = inputManager.InputVector.x;
            float yawRotation = yawInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, yawRotation, 0f);
        }

        private bool CanMove()
        {
            return !isMoving
                && (Time.time - lastMoveTime) >= (moveDuration + moveCooldown)
                && stamina.CanSwim();
        }
    }
}