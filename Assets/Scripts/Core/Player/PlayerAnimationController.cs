using UnityEngine;

namespace FaS.DiverGame
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator m_animLeft;
        [SerializeField] private Animator m_animRight;

        private readonly int MoveParameterHash = Animator.StringToHash("BeginStroke");

        public void SetMoveTrigger()
        {
            m_animLeft.SetTrigger(MoveParameterHash);
            m_animRight.SetTrigger(MoveParameterHash);
        }
    }
}