using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;

namespace slc.NIGHTSWIM.WaterSystem
{
    public class WaterSurfaceController : MonoBehaviour
    {
        //Public Properties
        public int Dimension = 10;
        public float UVScale = 2.0f;
        public Octave[] Octaves;

        //Mesh
        protected MeshFilter MeshFilter;
        protected Mesh Mesh;

        private Vector3[] m_cachedVertices;
        private NativeArray<Vector3> m_nativeVertices;

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

            // Initialize the NativeArray for job execution
            m_nativeVertices = new NativeArray<Vector3>((Dimension + 1) * (Dimension + 1), Allocator.Persistent);
            for (int i = 0; i < m_cachedVertices.Length; i++)
            {
                m_nativeVertices[i] = m_cachedVertices[i];
            }
        }

        public float GetHeight(Vector3 t_position)
        {
            Vector3 t_scale = new(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
            Vector3 t_localPos = Vector3.Scale(t_position - transform.position, t_scale);

            // Get base X and Z in float
            float lx = t_localPos.x;
            float lz = t_localPos.z;

            // Clamp inside bounds
            lx = Mathf.Clamp(lx, 0, Dimension);
            lz = Mathf.Clamp(lz, 0, Dimension);

            // Convert to grid corners
            Vector2Int[] t_points = new Vector2Int[]
            {
                new Vector2Int(Mathf.FloorToInt(lx), Mathf.FloorToInt(lz)),
                new Vector2Int(Mathf.FloorToInt(lx), Mathf.CeilToInt(lz)),
                new Vector2Int(Mathf.CeilToInt(lx), Mathf.FloorToInt(lz)),
                new Vector2Int(Mathf.CeilToInt(lx), Mathf.CeilToInt(lz)),
            };

            Vector3[] t_vertices = m_cachedVertices;
            float t_max = float.MinValue;
            float[] t_distances = new float[4];

            // Compute distances and max
            for (int i = 0; i < 4; i++)
            {
                Vector3 p = new(t_points[i].x, 0, t_points[i].y);
                t_distances[i] = Vector3.Distance(p, t_localPos);
                t_max = Mathf.Max(t_max, t_distances[i]);
            }

            float t_weightSum = Mathf.Epsilon;
            float t_heightSum = 0f;

            for (int i = 0; i < 4; i++)
            {
                float t_weight = t_max - t_distances[i];
                t_weightSum += t_weight;
                t_heightSum += t_vertices[Index(t_points[i].x, t_points[i].y)].y * t_weight;
            }

            return t_heightSum * transform.lossyScale.y / t_weightSum;
        }

        private Vector3[] GenerateVerts()
        {
            // Generate vertices for the water surface
            Vector3[] t_vertices = new Vector3[(Dimension + 1) * (Dimension + 1)];
            for (int x = 0; x <= Dimension; x++)
            {
                for (int z = 0; z <= Dimension; z++)
                {
                    t_vertices[Index(x, z)] = new Vector3(x, 0, z);
                }
            }

            return t_vertices;
        }

        private int[] GenerateTries()
        {
            int[] t_tries = new int[Dimension * Dimension * 6]; // 2 tries per tile
            int t = 0;

            for (int x = 0; x < Dimension; x++)
            {
                for (int z = 0; z < Dimension; z++)
                {
                    int i0 = Index(x, z);
                    int i1 = Index(x + 1, z);
                    int i2 = Index(x, z + 1);
                    int i3 = Index(x + 1, z + 1);

                    t_tries[t++] = i0; t_tries[t++] = i1; t_tries[t++] = i3;
                    t_tries[t++] = i0; t_tries[t++] = i3; t_tries[t++] = i2;
                }
            }

            return t_tries;
        }

        private Vector2[] GenerateUVs()
        {
            // Generate UVs for water mesh
            Vector2[] t_uvs = new Vector2[(Dimension + 1) * (Dimension + 1)];
            for (int x = 0; x <= Dimension; x++)
            {
                for (int z = 0; z <= Dimension; z++)
                {
                    t_uvs[Index(x, z)] = new Vector2((float)x / Dimension, (float)z / Dimension);
                }
            }
            return t_uvs;
        }

        private int Index(int x, int z)
        {
            return (x * (Dimension + 1)) + z;
        }

        private void Update()
        {
            // Create job data for UpdateWaterHeightJob
            UpdateWaterHeightJob t_updateJob = new()
            {
                Dimension = Dimension,
                Octaves = new NativeArray<Octave>(Octaves, Allocator.TempJob),
                Time = Time.time,
                Vertices = m_nativeVertices
            };

            // Schedule and complete the job
            JobHandle t_jobHandle = t_updateJob.Schedule(m_nativeVertices.Length, 64);
            t_jobHandle.Complete();

            // Transfer the updated vertices back to the Mesh
            for (int i = 0; i < m_nativeVertices.Length; i++)
            {
                m_cachedVertices[i] = m_nativeVertices[i];
            }

            // Update the mesh with the new vertices
            Mesh.vertices = m_cachedVertices;
            Mesh.RecalculateNormals();

            // Dispose of the temporary NativeArray
            t_updateJob.Octaves.Dispose();
        }

        private void OnDestroy()
        {
            // Make sure to dispose of the NativeArray when the object is destroyed
            if (m_nativeVertices.IsCreated)
            {
                m_nativeVertices.Dispose();
            }
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