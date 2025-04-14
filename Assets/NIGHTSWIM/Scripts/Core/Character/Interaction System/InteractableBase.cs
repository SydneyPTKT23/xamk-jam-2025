using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interactable Settings")]
        [SerializeField] private bool isInteractable = true;
        [SerializeField] private string tooltipMessage = "interact";

        public bool IsInteractable => isInteractable;
        public string TooltipMessage => tooltipMessage;

        public virtual void OnInteract()
        {
            Debug.Log("INTERACTED: " + gameObject.name);
        }
    }
}