using System;
using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem
{
    public class WaterSurfaceController : MonoBehaviour
    {
        //Public Properties
        public int Dimension = 10;
        public float UVScale = 2f;
        public Octave[] Octaves;

        //Mesh
        protected MeshFilter MeshFilter;
        protected Mesh Mesh;

        private Vector3[] m_cachedVertices;

        private void Start()
        {
            InitializeMesh();
        }

        private void InitializeMesh()
        {
            Mesh = new Mesh
            {
                name = gameObject.name,
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            m_cachedVertices = GenerateVerts();
            Mesh.vertices = m_cachedVertices;
            Mesh.triangles = GenerateTries();
            Mesh.uv = GenerateUVs();
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();

            MeshFilter = gameObject.AddComponent<MeshFilter>();
            MeshFilter.mesh = Mesh;
        }

        public float GetHeight(Vector3 position)
        {
            Vector3 scale = new(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
            Vector3 localPos = Vector3.Scale(position - transform.position, scale);

            // Get base X and Z in float
            float lx = localPos.x;
            float lz = localPos.z;

            // Clamp inside bounds
            lx = Mathf.Clamp(lx, 0, Dimension);
            lz = Mathf.Clamp(lz, 0, Dimension);

            // Convert to grid corners
            Vector2Int[] points = new Vector2Int[]
            {
                new Vector2Int(Mathf.FloorToInt(lx), Mathf.FloorToInt(lz)),
                new Vector2Int(Mathf.FloorToInt(lx), Mathf.CeilToInt(lz)),
                new Vector2Int(Mathf.CeilToInt(lx), Mathf.FloorToInt(lz)),
                new Vector2Int(Mathf.CeilToInt(lx), Mathf.CeilToInt(lz)),
            };

            Vector3[] verts = m_cachedVertices;
            float max = float.MinValue;
            float[] distances = new float[4];

            // Compute distances and max
            for (int i = 0; i < 4; i++)
            {
                Vector3 p = new(points[i].x, 0, points[i].y);
                distances[i] = Vector3.Distance(p, localPos);
                max = Mathf.Max(max, distances[i]);
            }

            float weightSum = Mathf.Epsilon;
            float heightSum = 0f;

            for (int i = 0; i < 4; i++)
            {
                float weight = max - distances[i];
                weightSum += weight;
                heightSum += verts[Index(points[i].x, points[i].y)].y * weight;
            }

            return heightSum * transform.lossyScale.y / weightSum;
        }

        private Vector3[] GenerateVerts()
        {
            Vector3[] verts = new Vector3[(Dimension + 1) * (Dimension + 1)];

            // Equally distributed verts
            for (int x = 0; x <= Dimension; x++)
            {
                for (int z = 0; z <= Dimension; z++)
                {
                    verts[Index(x, z)] = new Vector3(x, 0, z);
                }
            }

            return verts;
        }

        private int[] GenerateTries()
        {
            _ = (Dimension + 1) * (Dimension + 1);
            int[] tries = new int[Dimension * Dimension * 6]; // 2 tries per tile

            int t = 0;
            for (int x = 0; x < Dimension; x++)
            {
                for (int z = 0; z < Dimension; z++)
                {
                    int i0 = Index(x, z);
                    int i1 = Index(x + 1, z);
                    int i2 = Index(x, z + 1);
                    int i3 = Index(x + 1, z + 1);

                    tries[t++] = i0; tries[t++] = i3; tries[t++] = i1;
                    tries[t++] = i0; tries[t++] = i2; tries[t++] = i3;
                }
            }

            return tries;
        }

        private Vector2[] GenerateUVs()
        {
            int count = (Dimension + 1) * (Dimension + 1);
            Vector2[] uvs = new Vector2[count];

            for (int x = 0; x <= Dimension; x++)
            {
                for (int z = 0; z <= Dimension; z++)
                {
                    float u = x / UVScale % 2f;
                    float v = z / UVScale % 2f;

                    uvs[Index(x, z)] = new Vector2(u > 1 ? 2 - u : u, v > 1 ? 2 - v : v);
                }
            }

            return uvs;
        }

        private int Index(int x, int z)
        {
            return (x * (Dimension + 1)) + z;
        }

        private void Update()
        {
            float t_timer = Time.time;

            for (int x = 0; x <= Dimension; x++)
            {
                float normX = (float)x / Dimension;

                for (int z = 0; z <= Dimension; z++)
                {
                    float normZ = (float)z / Dimension;
                    float y = 0f;

                    for (int o = 0; o < Octaves.Length; o++)
                    {
                        Octave octave = Octaves[o];
                        if (octave.alternate)
                        {
                            float perl = Mathf.PerlinNoise(normX * octave.scale.x, normZ * octave.scale.y) * Mathf.PI * 2f;
                            y += Mathf.Cos(perl + (octave.speed.magnitude * t_timer)) * octave.height;
                        }
                        else
                        {
                            float perl = Mathf.PerlinNoise(
                                (normX * octave.scale.x) + (t_timer * octave.speed.x),
                                (normZ * octave.scale.y) + (t_timer * octave.speed.y)) - 0.5f;

                            y += perl * octave.height;
                        }
                    }

                    m_cachedVertices[Index(x, z)] = new Vector3(x, y, z);
                }
            }

            Mesh.vertices = m_cachedVertices;
            Mesh.RecalculateNormals();
        }

        [Serializable]
        public struct Octave
        {
            public Vector2 speed;
            public Vector2 scale;
            public float height;
            public bool alternate;
        }
    }
}