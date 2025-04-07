using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace slc.NIGHTSWIM.Audio
{
    public enum AmbientType
    {
        AMB_1,
        AMB_2,
        AMB_3,
        AMB_4,
        THUNDER
    }

    [RequireComponent(typeof(AudioSource))]
    public class AmbientController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private AudioClip[] soundList;
        private static AmbientController instance;
        private AudioSource audioSource;
        public float speed = 1.0f;

        private void Awake()
        {
            instance = this;
        }
        // Update is called once per frame
        void Update()
        {
            transform.LookAt(player);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        public static void PlayAmbience(AmbientType type, int vol)
        {
            float pitchVar = Random.Range(0.95f, 1.05f);
            instance.audioSource.pitch = pitchVar;
            instance.audioSource.PlayOneShot(instance.soundList[(int)type], vol);
        }
    }
}
