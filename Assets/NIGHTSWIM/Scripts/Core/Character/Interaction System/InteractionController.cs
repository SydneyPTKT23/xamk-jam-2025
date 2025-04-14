using slc.NIGHTSWIM.Input;
using slc.NIGHTSWIM.UI;
using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    public class InteractionController : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private float rayDistance = 2.0f;
        [SerializeField] private float raySphereRadius = 0.1f;
        [SerializeField] private LayerMask interactableLayer = ~0;

        [Header("UI")]
        [SerializeField] private InteractionPanel panel;

        private InputManager m_inputManager;
        private Camera m_camera;

        private bool m_isInteracting;
        private InteractableBase m_currentInteractable;

        private RaycastHit m_hitInfo;

        private void Awake()
        {
            // Cache the camera component
            m_camera = GetComponentInChildren<Camera>();
            m_inputManager = GetComponent<InputManager>();

            m_inputManager.OnInteractClicked += OnInteractInput;
        }

        private void Update()
        {
            // Check for interactables and handle input in one place
            CheckForInteractables();
            HandleInteractionInput();
        }

        private void CheckForInteractables()
        {
            // Perform a spherecast to detect interactable objects
            bool t_hitSomething = Physics.SphereCast(m_camera.transform.position, raySphereRadius, m_camera.transform.forward,
                out m_hitInfo, rayDistance, interactableLayer);

            // If an interactable object is detected, display the tooltip
            if (t_hitSomething && m_hitInfo.transform.TryGetComponent(out InteractableBase t_interactable))
            {
                m_currentInteractable = t_interactable;
                panel.SetLabel(t_interactable.TooltipMessage);
            }
            else
            {
                // Reset the UI if no interactable object is found
                panel.ResetUI();
                m_currentInteractable = null;
            }

            // Visualize the ray in the editor for debugging
            Debug.DrawRay(m_camera.transform.position, m_camera.transform.forward * rayDistance, t_hitSomething ? Color.green : Color.red);
        }

        private void HandleInteractionInput()
        {
            // If the player is interacting and the interactable is valid, invoke its interaction method
            if (m_isInteracting && m_currentInteractable != null && m_currentInteractable.IsInteractable)
            {
                m_currentInteractable.OnInteract();
                m_currentInteractable = null;
                m_isInteracting = false;
            }
        }

        private void OnInteractInput()
        {
            // If an interactable is found, set the flag to start interacting
            if (m_currentInteractable != null)
            {
                m_isInteracting = true;
            }
        }
    }
}