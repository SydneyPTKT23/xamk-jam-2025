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
        public AudioSource AudioSource_2;

        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            PlaySoundEffect(SoundType.SPLASH, 1.5f);
            PlayLoop1(SoundType.WAVES, 1.5f);
            PlayLoop2(SoundType.RAIN, 1);
        }

        public static void PlaySoundEffect(SoundType type, float vol)
        {
            float pitchVar = Random.Range(0.95f, 1.05f);
            instance.audioSource.pitch = pitchVar;
            instance.audioSource.PlayOneShot(instance.soundList[(int) type], vol);
        }

        public static void PlayLoop1(SoundType type, float vol)
        {
            instance.audioSource.loop = true;
            instance.audioSource.PlayOneShot(instance.soundList[(int)type], vol);
        }
        public static void PlayLoop2(SoundType type, float vol)
        {
            instance.AudioSource_2.loop = true;
            instance.AudioSource_2.PlayOneShot(instance.soundList[(int)type], vol);
        }
    }
}
