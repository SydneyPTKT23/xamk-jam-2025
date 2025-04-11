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
        [ReadOnly] public NativeArray<WaterSurfaceController.Octave> Octaves;
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
            for (int o = 0; o < Octaves.Length; o++)
            {
                WaterSurfaceController.Octave t_octave = Octaves[o];
                if (t_octave.alternate)
                {
                    float t_perlin = Mathf.PerlinNoise(t_normX * t_octave.scale.x, t_normZ * t_octave.scale.y) * Mathf.PI * 2f;
                    y += Mathf.Cos(t_perlin + (t_octave.speed.magnitude * Time)) * t_octave.height;
                }
                else
                {
                    float t_perlin = Mathf.PerlinNoise(
                        (t_normX * t_octave.scale.x) + (Time * t_octave.speed.x),
                        (t_normZ * t_octave.scale.y) + (Time * t_octave.speed.y)) - 0.5f;

                    y += t_perlin * t_octave.height;
                }
            }

            // Update the vertex position in the correct order
            Vertices[t_index] = new Vector3(x, y, z);
        }
    }
}