using System;
using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem.Surface
{
    [CreateAssetMenu(fileName = "WaterSettings", menuName = "NIGHTSWIM/Water System/Water Settings", order = 0)]
    public class WaterSettings : ScriptableObject
    {
        [Range(0.001f, 1f)]
        public float Frequency = 0.01f;
        [Range(0.0f, 10f)]
        public float Amplitude = 1.0f;
    }
}