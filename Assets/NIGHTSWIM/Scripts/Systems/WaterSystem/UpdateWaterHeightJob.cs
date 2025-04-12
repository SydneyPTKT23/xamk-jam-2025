using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;

namespace slc.NIGHTSWIM.WaterSystem
{
    [BurstCompile]
    public struct UpdateWaterHeightJob : IJobParallelFor
    {
        [ReadOnly] public int Dimension;
        [ReadOnly] public NativeArray<WaterSettings.WaveLayer> WaveLayers;
        [ReadOnly] public float Time;
        public NativeArray<Vector3> Vertices;

        public void Execute(int t_index)
        {
            // Calculate x and z based on the index
            int x = t_index % (Dimension + 1);  // This gives the column in the grid
            int z = t_index / (Dimension + 1);  // This gives the row in the grid

            float t_normX = (float)x / Dimension;
            float t_normZ = (float)z / Dimension;

            float y = 0f;

            // Loop through octaves to calculate the height for this vertex
            for (int o = 0; o < WaveLayers.Length; o++)
            {
                WaterSettings.WaveLayer t_waveLayers = WaveLayers[o];
                if (t_waveLayers.alternate)
                {
                    float t_perlin = Mathf.PerlinNoise(t_normX * t_waveLayers.scale.x, t_normZ * t_waveLayers.scale.y) * Mathf.PI * 2f;
                    y += Mathf.Cos(t_perlin + (t_waveLayers.speed.magnitude * Time)) * t_waveLayers.height;
                }
                else
                {
                    float t_perlin = Mathf.PerlinNoise(
                        (t_normX * t_waveLayers.scale.x) + (Time * t_waveLayers.speed.x),
                        (t_normZ * t_waveLayers.scale.y) + (Time * t_waveLayers.speed.y)) - 0.5f;

                    y += t_perlin * t_waveLayers.height;
                }
            }

            // Update the vertex position in the correct order
            Vertices[t_index] = new Vector3(x, y, z);
        }
    }
}