using UnityEngine;

namespace FaS.DiverGame.Core
{
    public class EdibleManager : MonoBehaviour
    {
        public SpriteRenderer m_renderer;
        public Sprite currentFoodSprite;

        private PlayerAnimationController m_controller;

        private void Start()
        {
            m_controller = GetComponentInParent<PlayerAnimationController>();
        }

        public void StartEating(Sprite t_foodSprite)
        {
            currentFoodSprite = t_foodSprite;
            m_controller.SetEatTrigger();
        }

        public void ShowFoodInHand()
        {
            if (m_renderer != null)
            {
                m_renderer.sprite = currentFoodSprite;
                m_renderer.enabled = true;
            }
        }

        public void FinishEating()
        {
            if (m_renderer != null)
            {
                m_renderer.sprite = null;
                m_renderer.enabled = false;
            }
        }
    }
}