using slc.NIGHTSWIM.Core;
using UnityEngine;

namespace slc.NIGHTSWIM
{
    public class Meal : InteractableBase
    {
        [SerializeField] private SpriteRenderer m_renderer;

        private EdibleManager m_eatingController;

        private void Start()
        {
            m_renderer = GetComponentInChildren<SpriteRenderer>();
            m_eatingController = FindObjectOfType<EdibleManager>();
        }

        public override void OnInteract()
        {
            base.OnInteract();

            if (m_eatingController != null && m_renderer.sprite != null)
            {
                m_eatingController.StartEating(m_renderer.sprite);
            }
        }
    }
}