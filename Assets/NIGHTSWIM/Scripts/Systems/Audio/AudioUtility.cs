using UnityEngine;
using UnityEngine.Audio;

namespace slc.NIGHTSWIM.Systems
{
    public class AudioUtility
    {
        private static AudioManager s_audioManager;

        public const string MASTER_VOLUME_PARAM = "MasterVolume";
        public const string MUSIC_VOLUME_PARAM = "MusicVolume";
        public const string SFX_VOLUME_PARAM = "SoundsVolume";
        public const string AMBIENT_VOLUME_PARAM = "AmbientVolume";

        static AudioManager AudioManager
        {
            get
            {
                if (s_audioManager == null)
                    s_audioManager = GameObject.FindObjectOfType<AudioManager>();
                return s_audioManager;
            }
        }

        // Replace these with the real audio groups
        public enum AudioGroups
        {
            Master,
            Music,
            SFX,
            Ambient,
        }

        /// <summary>
        /// Spawns a temporary audio source at a world position and plays a 3D clip.
        /// </summary>
        public static void CreateSFX(
            AudioClip t_clip,
            Vector3 t_position,
            AudioGroups t_audioGroup,
            float t_spatialBlend,
            float t_rolloffDistanceMin = 1.0f,
            float t_rolloffDistanceMax = 20.0f,
            AudioRolloffMode t_rolloffMode = AudioRolloffMode.Linear)
        {
            if (t_clip == null)
            {
                Debug.LogWarning("AudioUtility: Tried to play a null AudioClip.");
                return;
            }

            GameObject t_impactSfxInstance = new("SFX_" + t_clip.name);
            t_impactSfxInstance.transform.position = t_position;

            AudioSource t_source = t_impactSfxInstance.AddComponent<AudioSource>();
            t_source.clip = t_clip;
            t_source.spatialBlend = t_spatialBlend;
            t_source.minDistance = t_rolloffDistanceMin;
            t_source.maxDistance = t_rolloffDistanceMax;
            t_source.rolloffMode = t_rolloffMode;

            t_source.outputAudioMixerGroup = GetAudioGroup(t_audioGroup);

            t_source.Play();
            Object.Destroy(t_impactSfxInstance, t_clip.length);
        }

        // Use this instead if the sound is 2D and doesn't need a world position
        public static void CreateSFX(AudioClip t_clip, AudioGroups t_audioGroup)
        {
            CreateSFX(t_clip, Vector3.zero, t_audioGroup, 0f);
        }

        public static AudioMixerGroup GetAudioGroup(AudioGroups t_audioGroup)
        {
            AudioManager t_audioManager = AudioManager;
            if (t_audioManager == null)
                return null;

            string t_groupName = t_audioGroup.ToString();
            AudioMixerGroup[] t_groups = t_audioManager.FindMatchingGroups(t_groupName);

            if (t_groups != null && t_groups.Length > 0)
                return t_groups[0];

            Debug.LogWarning("AudioUtility: Couldn't find audio group for " + t_groupName);
            return null;
        }

        public static void SetVolume(string t_parameter, float t_value)
        {
            float t_clampedValue = Mathf.Max(t_value, 0.001f);
            float t_valueInDb = Mathf.Log10(t_clampedValue) * 20;

            AudioManager t_audioManager = AudioManager;
            if (t_audioManager == null)
                return;

            t_audioManager.SetFloat(t_parameter, t_valueInDb);
        }

        public static float GetVolume(string t_parameter)
        {
            AudioManager t_audioManager = AudioManager;
            if (t_audioManager != null)
            {
                t_audioManager.GetFloat(t_parameter, out float t_valueInDb);
                return Mathf.Pow(10f, t_valueInDb / 20.0f);
            }

            return 1.0f;
        }

        // Shortcuts for individual sliders
        public static void SetMasterVolume(float t_value) => SetVolume(MASTER_VOLUME_PARAM, t_value);
        public static float GetMasterVolume() => GetVolume(MASTER_VOLUME_PARAM);

        public static void SetMusicVolume(float t_value) => SetVolume(MUSIC_VOLUME_PARAM, t_value);
        public static float GetMusicVolume() => GetVolume(MUSIC_VOLUME_PARAM);

        public static void SetSFXVolume(float t_value) => SetVolume(SFX_VOLUME_PARAM, t_value);
        public static float GetSFXVolume() => GetVolume(SFX_VOLUME_PARAM);

        public static void SetAmbientVolume(float t_value) => SetVolume(AMBIENT_VOLUME_PARAM, t_value);
        public static float GetAmbientVolume() => GetVolume(AMBIENT_VOLUME_PARAM);
    }
}
