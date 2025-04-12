using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem
{
    [CreateAssetMenu(fileName = "WaterSettings", menuName = "NIGHTSWIM/Water System/Water Settings", order = 1)]
    public class WaterSettings : ScriptableObject
    {
        public WaveLayer[] WaveLayers;

        [System.Serializable]
        public struct WaveLayer
        {
            public Vector2 speed;
            public Vector2 scale;
            public float height;
            public bool alternate;
        }
    }
}