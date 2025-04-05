using UnityEngine;

namespace FaS.DiverGame
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Effects")]
        public CameraRecoilOnStroke recoil;
        public CameraFOVBurst burst;
        public CameraShake shake;

        public void TriggerEffects()
        {
            if (recoil != null) recoil.Activate();
            if (burst != null) burst.Activate();
            //shake?.StartShake();
        }
    }
}