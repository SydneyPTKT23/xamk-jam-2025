using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FaS.DiverGame.Input
{
    public class InputHandler : MonoBehaviour, Controls.IGameActions
    {
        public Controls Controls { get; private set; }
        public Controls.GameActions GameActions { get; private set; }

        public Vector2 InputVector { get; private set; }
        public bool HasInputX => InputVector.x != 0f;
        public bool HasInputY => InputVector.y != 0f;

        private readonly Coroutine m_disableActionCoroutine;

        #region Built-In Methods
        private void Awake()
        {
            Controls = new Controls();
            GameActions = Controls.Game;
        }

        private void OnEnable()
        {
            GameActions.SetCallbacks(this);
            Controls.Enable();
        }

        private void OnDisable()
        {
            GameActions.RemoveCallbacks(this);
            Controls.Disable();
        }
        #endregion

        public void OnMove(InputAction.CallbackContext t_context)
        {
            InputVector = t_context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext t_context)
        {
            if (t_context.performed)
            {
                t_context.ReadValueAsButton();
            }
        }

        #region Utilities
        public void DisableActionFor(InputAction t_action, float t_seconds)
        {
            if (m_disableActionCoroutine != null)
            {
                StopCoroutine(m_disableActionCoroutine);
            }

            StartCoroutine(DisableAction(t_action, t_seconds));
        }

        private IEnumerator DisableAction(InputAction t_action, float t_seconds)
        {
            if (t_action == null)
            {
                yield break;
            }

            t_action.Disable();
            yield return new WaitForSeconds(t_seconds);
            t_action.Enable();
        }
        #endregion
    }
}