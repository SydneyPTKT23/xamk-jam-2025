using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaS.DiverGame
{
    public enum SoundType
    {
        STROKE,
        EAT,
        AMB_1,
        AMB_2,
        AMB_3,
        AMB_4,
        POSRESET
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundsOnPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] soundList;
        private static SoundsOnPlayer instance;
        private AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public static void PlaySound(SoundType type, int vol)
        {
            float pitchVar = Random.Range(0.95f, 1.05f);
            instance.audioSource.pitch = pitchVar;
            instance.audioSource.PlayOneShot(instance.soundList[(int) type], vol);
        }
    }
}
