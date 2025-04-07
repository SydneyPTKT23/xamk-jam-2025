using slc.NIGHTSWIM.Input;
using slc.NIGHTSWIM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    public class InteractionController : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private float rayDistance = 2.0f;
        [SerializeField] private float raySphereRadius = 0.1f;
        [SerializeField] private LayerMask interactableLayer = ~0;

        [Space, Header("UI")]
        [SerializeField] private InteractionPanel panel;

        private InputHandler m_inputHandler;
        private Camera m_camera;

        private bool isInteracting;

        public InteractableBase m_interactable;

        private void Awake()
        {
            m_camera = GetComponentInChildren<Camera>();
            m_inputHandler = GetComponent<InputHandler>();

            m_inputHandler.OnInteractClicked += StartInput;
        }

        private void Update()
        {
            CheckForInteractables();
            CheckForInput();
        }

        private void CheckForInteractables()
        {
            Ray t_ray = new(m_camera.transform.position, m_camera.transform.forward);
            bool t_hitSomething = Physics.SphereCast(t_ray, raySphereRadius, out RaycastHit t_hitInfo, rayDistance, interactableLayer);

            if (t_hitSomething)
            {
                InteractableBase t_interactable = t_hitInfo.transform.GetComponent<InteractableBase>();

                if (t_interactable != null)
                {
                    m_interactable = t_interactable;
                    panel.SetLabel(t_interactable.TooltipMessage);
                }
            }
            else
            {
                panel.ResetUI();
                ResetInteractable();
            }

            Debug.DrawRay(t_ray.origin, t_ray.direction * rayDistance, t_hitSomething ? Color.green : Color.red);
        }

        private void Interact()
        {
            m_interactable.OnInteract();
            ResetInteractable();
        }

        private void StartInput()
        {
            if(m_interactable == null)
                return;

            isInteracting = true;
        }

        private void CheckForInput()
        {
            if (isInteracting)
            {
                if (!m_interactable.IsInteractable)
                    return;

                Interact();
                isInteracting = false;
            }
        }

        private void ResetInteractable()
        {
            m_interactable = null;
        }
    }
}