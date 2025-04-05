using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaS.DiverGame
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float shakeAmount = 0.05f;
        [SerializeField] private float shakeDuration = 0.15f;

        private float shakeTimer = 0f;
        private Vector3 defaultPos;

        private void Start()
        {
            defaultPos = transform.localPosition;
        }

        public void Activate()
        {
            shakeTimer = shakeDuration;
        }

        private void Update()
        {
            if (shakeTimer > 0)
            {
                Vector3 t_shakeOffset = Random.insideUnitSphere * shakeAmount;
                transform.localPosition = defaultPos + t_shakeOffset;
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime * 5.0f);
            }
        }
    }
}