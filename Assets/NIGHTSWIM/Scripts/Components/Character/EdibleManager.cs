using slc.NIGHTSWIM.Audio;
using UnityEngine;

namespace slc.NIGHTSWIM.Core
{
    public class EdibleManager : MonoBehaviour
    {
        public SpriteRenderer m_renderer;
        public Sprite currentFoodSprite;
        public ParticleSystem eat;

        private PlayerAnimationController m_controller;

        private void Start()
        {
            m_controller = GetComponentInParent<PlayerAnimationController>();
            eat = GameObject.Find("eat").GetComponent<ParticleSystem>();
            eat.Stop();
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
                eat.Play();
                SoundsOnPlayer.PlaySoundEffect(SoundType.EAT, 1.5f);
            }
        }

        public void FinishEating()
        {
            if (m_renderer != null)
            {
                m_renderer.sprite = null;
                m_renderer.enabled = false;
                eat.Stop();
            }
        }
    }
}