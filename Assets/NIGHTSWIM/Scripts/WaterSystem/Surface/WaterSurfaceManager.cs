using System.Collections.Generic;
using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem.Surface
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class WaterSurfaceManager : MonoBehaviour
    {
        public List<WaterSettings> waterSettings = new();

        [Range(1f, 10000f)] public float Size = 1000.0f;
        [Range(10, 255)] public int Segments = 100;

        public bool UseThreshold = false;
        public float xshift = 0.0f;
        public float yshift = 0.0f;

        [Range(0f, 1f)]
        public float waveSpeed = 0f;

        private Mesh mesh;

        private void OnDrawGizmos()
        {
            GenerateMesh();
        }

        void GenerateMesh()
        {
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = "Terrain Mesh";
            }
            else { mesh.Clear(); }

            List<int> tris = new();
            List<Vector2> uvs = new();
            List<Vector3> vertices = new();

            float delta = Size / (Segments - 1);

            //Generate vertices
            for (int i = 0; i < Segments; i++)
            {
                for (int j = 0; j < Segments; j++)
                {
                    float x = i * delta;
                    float y = j * delta;

                    //Perlin Noise
                    float noiseValue = 0.0f;
                    for (int k = 0; k < waterSettings.Count; k++)
                    {
                        noiseValue += waterSettings[k].Amplitude * 2.0f *
                                        (Mathf.PerlinNoise((x + xshift) * waterSettings[k].Frequency,
                                                           (y + yshift) * waterSettings[k].Frequency) - 0.5f);
                    }

                    if (UseThreshold)
                    {
                        if ((noiseValue < 0.0f))
                        {
                            noiseValue = 0.0f;
                        }
                    }

                    vertices.Add(new Vector3(x, noiseValue, y));
                    uvs.Add(new Vector2((x + xshift / 10) / Size, (y + yshift / 10) / Size));

                }
            }
            //Generate triangles
            for (int i = 0; i < Segments - 1; i++)
            {
                for (int j = 0; j < Segments - 1; j++)
                {
                    //"Upper Left"
                    int ul = j * Segments + i;
                    //"Upper Right"
                    int ur = ul + 1;

                    //"Lower Left"
                    int ll = ul + Segments;
                    //"Lower Right"
                    int lr = ll + 1;

                    //Triangles
                    tris.Add(ll);
                    tris.Add(ul);
                    tris.Add(ur);

                    tris.Add(ll);
                    tris.Add(ur);
                    tris.Add(lr);
                }
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.SetUVs(0, uvs);

            GetComponent<MeshFilter>().sharedMesh = mesh;

        }

        void FixedUpdate()
        {
            GenerateMesh();
            xshift += waveSpeed;
            yshift += waveSpeed;
        }

        public float GetHeightAtWorldPosition(Vector3 worldPos)
        {
            float x = worldPos.x;
            float z = worldPos.z;

            float noiseValue = 0.0f;
            for (int k = 0; k < waterSettings.Count; k++)
            {
                float frequency = waterSettings[k].Frequency;
                float amplitude = waterSettings[k].Amplitude;

                noiseValue += amplitude * 2.0f *
                    (Mathf.PerlinNoise((x + xshift) * frequency, (z + yshift) * frequency) - 0.5f);
            }

            if (UseThreshold && noiseValue < 0f)
                noiseValue = 0f;

            return noiseValue;
        }
    }
}
