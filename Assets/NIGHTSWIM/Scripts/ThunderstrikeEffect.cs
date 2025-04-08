using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace slc.NIGHTSWIM.Core
{
    public class ThunderstrikeEffect : MonoBehaviour
    {
        [Header("Exposure Settings")]
        public Light worldLight;
        public float flashIntensity = 3.0f;
        public float normalIntensity = 1.0f;
        public float flashDuration = 0.2f;
        public AnimationCurve flashCurve = AnimationCurve.EaseInOut(0, 3, 1, 1);

        public Volume postVolume;
        public float bloomFlashIntensity = 1.0f;
        private Bloom bloom;

        [Header("Thunder Settings")]
        public AudioSource thunderAudio;
        public Vector2 thunderDelayRange = new(0.5f, 3.0f);

        [Header("Camera Shake")]
        public Transform cameraTransform;
        public float shakeDuration = 0.2f;
        public float shakeIntensity = 0.3f;

        private void Start()
        {
            if (postVolume != null)
                postVolume.profile.TryGet(out bloom);
        }

        public void TriggerThunder()
        {
            StartCoroutine(ThunderRoutine());
        }

        IEnumerator ThunderRoutine()
        {
            yield return StartCoroutine(FlashLight());
            if (cameraTransform != null) StartCoroutine(ShakeCamera());

            float t_delay = Random.Range(thunderDelayRange.x, thunderDelayRange.y);
            yield return new WaitForSeconds(t_delay);

            if (thunderAudio != null) thunderAudio.Play();
        }

        IEnumerator FlashLight()
        {
            float t = 0f;

            while (t < flashDuration)
            {
                float t_curveValue = flashCurve.Evaluate(t / flashDuration);
                if (worldLight != null)
                    worldLight.intensity = t_curveValue * flashIntensity;

                if (bloom != null)
                    bloom.intensity.value = Mathf.Lerp(0f, bloomFlashIntensity, t_curveValue);

                t += Time.deltaTime;
                yield return null;
            }

            if (worldLight != null)
                worldLight.intensity = normalIntensity;

            if (bloom != null)
                bloom.intensity.value = 0f;
        }

        IEnumerator ShakeCamera()
        {
            Vector3 t_originalPos = cameraTransform.localPosition;
            float t = 0f;
            while (t < shakeDuration)
            {
                cameraTransform.localPosition = t_originalPos + Random.insideUnitSphere * shakeIntensity;
                t += Time.deltaTime;
                yield return null;
            }

            cameraTransform.localPosition = t_originalPos;
        }
    }
}