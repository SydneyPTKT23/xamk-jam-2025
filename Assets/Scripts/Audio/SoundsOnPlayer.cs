using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

namespace FaS.DiverGame.Audio
{
    public enum SoundType
    {
        SPLASH,
        STROKE,
        EAT,
        WAVES,
        RAIN
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundsOnPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] soundList;
        private static SoundsOnPlayer instance;
        private AudioSource audioSource;

        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public static void PlaySoundEffect(SoundType type, int vol)
        {
            float pitchVar = Random.Range(0.95f, 1.05f);
            instance.audioSource.pitch = pitchVar;
            instance.audioSource.PlayOneShot(instance.soundList[(int) type], vol);
            Debug.Log("Splash");
        }

        public static void PlayLoop(SoundType type, int vol)
        {
            instance.audioSource.loop = true;
            instance.audioSource.PlayOneShot(instance.soundList[(int)type], vol);
        }
    }
}
