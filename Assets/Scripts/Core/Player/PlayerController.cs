using FaS.DiverGame;
using FaS.DiverGame.Input;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private float moveCooldown = 0.5f;
    [SerializeField] private AnimationCurve swimCurve = AnimationCurve.EaseInOut(0f, 0f, 1.0f, 1.0f);

    [Space, Header("Turning")]
    [SerializeField] private float turnSpeed = 90.0f;

    [Space, Header("Smooth")]
    [SerializeField] private float smoothInputSpeed = 10.0f;

    private Vector3 m_currentDirection;
    private bool m_isMoving = false;

    private float m_moveTimer = 0f;
    private float m_elapsedMoveTime = Mathf.NegativeInfinity;

    private CharacterController m_characterController;
    private InputHandler m_inputHandler;

    private CameraController m_cameraController;

    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_inputHandler = GetComponent<InputHandler>();

        m_cameraController = GetComponentInChildren<CameraController>();

        if (swimCurve == null || swimCurve.length == 0)
        {
            swimCurve = new AnimationCurve(
                new Keyframe(0f, 1.0f),
                new Keyframe(1.0f, 0f)
            );
        }

        m_currentDirection = transform.forward;
    }

    private void Update()
    {
        RotatePlayer();

        if (CanMove() && m_inputHandler.HasInputY)
        {
            StartMovement();
        }

        if (m_isMoving)
        {
            m_moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(m_moveTimer / moveDuration);
            float t_curveMultiplier = swimCurve.Evaluate(t);
            float t_speed = moveSpeed * t_curveMultiplier;

            Vector3 t_movement = m_currentDirection * (t_speed * t_curveMultiplier);
            m_characterController.Move(t_movement * Time.deltaTime);

            if (m_moveTimer >= moveDuration)
            {
                m_isMoving = false;
            }
        }
    }

    private void StartMovement()
    {
        m_cameraController.TriggerEffects();

        m_moveTimer = 0f;
        m_elapsedMoveTime = Time.time;
        m_isMoving = true;

        m_currentDirection = m_inputHandler.InputVector.y > 0 ? transform.forward : -transform.forward;
    }

    private bool CanMove()
    {
        return !m_isMoving && (Time.time - m_elapsedMoveTime) >= (moveDuration + moveCooldown);
    }

    void RotatePlayer()
    {
        float t_yawInput = m_inputHandler.InputVector.x;
        float t_yawDelta = t_yawInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, t_yawDelta, 0f);
    }
}