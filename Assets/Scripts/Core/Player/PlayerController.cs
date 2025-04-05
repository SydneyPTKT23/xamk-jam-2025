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

    private Vector2 m_smoothInputVector;
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
    }

    private void Update()
    {
        RotatePlayer();

        m_smoothInputVector = Vector2.Lerp(m_smoothInputVector, m_inputHandler.InputVector, Time.deltaTime * smoothInputSpeed);
        Vector3 t_direction = m_smoothInputVector.y * transform.forward;

        if (!m_isMoving && Time.time - m_elapsedMoveTime >= moveCooldown && m_inputHandler.HasInputY)
        {
            StartMove();
        }

        if (m_isMoving)
        {
            m_moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(m_moveTimer / moveDuration);
            float t_curveMultiplier = swimCurve.Evaluate(t);
            float t_speed = moveSpeed * t_curveMultiplier;

            Vector3 t_movement = t_direction * (t_speed * t_curveMultiplier);
            m_characterController.Move(t_movement * Time.deltaTime);

            if (m_moveTimer >= moveDuration)
            {
                m_isMoving = false;
            }
        }
    }

    private void StartMove()
    {
        m_cameraController.TriggerEffects();

        m_moveTimer = 0f;
        m_elapsedMoveTime = Time.time;
        m_isMoving = true;
    }

    void RotatePlayer()
    {
        float t_yawInput = m_inputHandler.InputVector.x;
        float t_yawDelta = t_yawInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, t_yawDelta, 0f);
    }
}