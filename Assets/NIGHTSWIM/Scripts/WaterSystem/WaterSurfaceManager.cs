using System.Collections.Generic;
using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class WaterSurfaceManager : MonoBehaviour
    {
        public List<WaterSettings> waterSettings = new();

        [Range(1f, 10000f)] public float size = 1000.0f;
        [Range(10, 255)] public int segments = 100;

        public bool useThreshold = false;

        [Range(0f, 1f)]
        public float waveSpeed = 0.1f;
        public Vector2 waveDirection = new(1f, 0f);

        private MeshFilter m_meshFilter;
        private Mesh m_mesh;
        private Vector3[] m_vertices;
        private Vector2[] m_uvs;
        private int[] m_triangles;

        private Vector2 waveOffset = Vector2.zero;

        #region Built-In Methods
        private void Awake()
        {
            waveDirection.Normalize();
            m_meshFilter = GetComponent<MeshFilter>();
            GenerateMesh();
        }

        private void FixedUpdate()
        {
            // float smoothedSpeed = Mathf.Lerp(waveOffset.magnitude, waveSpeed, 0.1f);
            // ^ will cause the water to Made in Heaven if added to the calc below ^
            waveOffset += waveDirection * Time.fixedDeltaTime;
            UpdateMeshHeights();
        }
        #endregion

        private void GenerateMesh()
        {
            m_mesh = new Mesh { name = "Water Surface Mesh" };
            m_vertices = new Vector3[segments * segments];
            m_uvs = new Vector2[m_vertices.Length];
            m_triangles = new int[(segments - 1) * (segments - 1) * 6];

            float t_delta = size / (segments - 1);

            // Vertices and UVs
            for (int y = 0, i = 0; y < segments; y++)
            {
                for (int x = 0; x < segments; x++, i++)
                {
                    float t_xPos = x * t_delta;
                    float t_zPos = y * t_delta;
                    m_vertices[i] = new Vector3(t_xPos, 0f, t_zPos);
                    m_uvs[i] = new Vector2(t_xPos / size, t_zPos / size);
                }
            }

            // Triangles
            int t = 0;
            for (int y = 0; y < segments - 1; y++)
            {
                for (int x = 0; x < segments - 1; x++)
                {
                    int i = y * segments + x;

                    m_triangles[t++] = i;
                    m_triangles[t++] = i + segments;
                    m_triangles[t++] = i + 1;

                    m_triangles[t++] = i + 1;
                    m_triangles[t++] = i + segments;
                    m_triangles[t++] = i + segments + 1;
                }
            }

            m_mesh.vertices = m_vertices;
            m_mesh.triangles = m_triangles;
            m_mesh.uv = m_uvs;
            m_mesh.RecalculateNormals();

            m_meshFilter.sharedMesh = m_mesh;

            UpdateMeshHeights();
        }

        private void UpdateMeshHeights()
        {
            float t_delta = size / (segments - 1);

            // Update mesh vertices and UVs based on wave offset
            for (int y = 0, i = 0; y < segments; y++)
            {
                for (int x = 0; x < segments; x++, i++)
                {
                    float t_xPos = x * t_delta;
                    float t_zPos = y * t_delta;

                    float t_height = 0f;
                    foreach (var t_setting in waterSettings)
                    {
                        float t_frequency = t_setting.Frequency;
                        float t_amplitude = t_setting.Amplitude;

                        float perlinX = (t_xPos + waveOffset.x) * t_frequency;
                        float perlinZ = (t_zPos + waveOffset.y) * t_frequency;

                        t_height += t_amplitude * 2.0f * (Mathf.PerlinNoise(perlinX, perlinZ) - 0.5f);
                    }

                    if (useThreshold && t_height < 0f)
                        t_height = 0f;

                    m_vertices[i].y = t_height;

                    // Move UVs based on wave offset for scrolling effect
                    m_uvs[i] = new Vector2((t_xPos / size) + waveOffset.x / size, (t_zPos / size) + waveOffset.y / size);
                }
            }

            m_mesh.vertices = m_vertices;
            m_mesh.uv = m_uvs;  // Update UVs to make the texture move with the waves
            m_mesh.RecalculateNormals();
        }

        public float GetHeightAtWorldPosition(Vector3 t_worldPos)
        {
            float x = t_worldPos.x;
            float z = t_worldPos.z;

            float t_noiseValue = 0.0f;
            foreach (var t_setting in waterSettings)
            {
                float t_frequency = t_setting.Frequency;
                float t_amplitude = t_setting.Amplitude;

                float perlinX = (x + waveOffset.x) * t_frequency;
                float perlinZ = (z + waveOffset.y) * t_frequency;

                t_noiseValue += t_amplitude * 2.0f * (Mathf.PerlinNoise(perlinX, perlinZ) - 0.5f);
            }

            if (useThreshold && t_noiseValue < 0f)
                t_noiseValue = 0f;

            return t_noiseValue;
        }

        public void SetWaveDirection(Vector2 dir)
        {
            waveDirection = dir.normalized;
        }
    }
}