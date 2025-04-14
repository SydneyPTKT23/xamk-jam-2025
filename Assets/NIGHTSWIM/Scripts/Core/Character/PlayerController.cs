using slc.NIGHTSWIM.Input;
using slc.NIGHTSWIM.WaterSystem;
using UnityEngine;

namespace slc.NIGHTSWIM
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

        private CharacterController characterController;
        private InputManager inputManager;
        private PlayerAnimationController animationController;
        private CameraController cameraController;

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

            currentDirection = transform.forward;

            // Ensure default swim curve if not set
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

            characterController.Move(speed * Time.deltaTime * currentDirection);

            if (moveTimer >= moveDuration)
            {
                isMoving = false;
            }
        }

        private void StartMovement()
        {
            cameraController.TriggerEffects();
            audioSource.PlayOneShot(strokeSfx);

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
            return !isMoving && (Time.time - lastMoveTime) >= (moveDuration + moveCooldown);
        }
    }
}