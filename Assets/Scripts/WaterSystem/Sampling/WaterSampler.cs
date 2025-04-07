using slc.NIGHTSWIM.WaterSystem.Surface;
using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem.Sampling
{
    public class WaterSampler : MonoBehaviour
    {
        public WaterSettings waterSettings;
        /*
        public float SampleHeight(Vector3 position)
        {
            float height = 0f;

            // Iterate through all wave layers to calculate height
            for (int i = 0; i < waterSettings.waveLayers.Count; i++)
            {
                WaterSettings.WaveLayer layer = waterSettings.waveLayers[i];
                float x = position.x + waterSettings.xShift;
                float y = position.z + waterSettings.yShift;
                height += layer.amplitude * Mathf.PerlinNoise(x * layer.frequency, y * layer.frequency);
            }

            if (waterSettings.useThreshold && height < waterSettings.threshold)
            {
                height = waterSettings.threshold;
            }

            return height;
        }*/
    }
}