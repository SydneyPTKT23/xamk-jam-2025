using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace slc.NIGHTSWIM.WaterSystem
{
    public class WaterChunk : MonoBehaviour
    {
        public int ChunkSize = 128; // number of quads along one edge
        public Vector2Int ChunkCoord; // chunk grid position
        public int WorldSize = 1024;  // total world size for global coord scaling
        public WaterSettings WaterSettings;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private Vector3[] _cachedVertices;
        private NativeArray<Vector3> _nativeVertices;

        public int VertexCount => (ChunkSize + 1) * (ChunkSize + 1);

        void Start()
        {
            InitializeMesh();
        }

        void InitializeMesh()
        {
            _mesh = new Mesh
            {
                name = $"WaterChunk_{ChunkCoord.x}_{ChunkCoord.y}",
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            _cachedVertices = GenerateVertices();
            _nativeVertices = new NativeArray<Vector3>(_cachedVertices.Length, Allocator.Persistent);
            for (int i = 0; i < _cachedVertices.Length; i++)
                _nativeVertices[i] = _cachedVertices[i];

            _mesh.vertices = _cachedVertices;
            _mesh.triangles = GenerateTriangles();
            _mesh.uv = GenerateUVs();
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = _mesh;
        }

        Vector3[] GenerateVertices()
        {
            var verts = new Vector3[(ChunkSize + 1) * (ChunkSize + 1)];
            int offsetX = ChunkCoord.x * ChunkSize;
            int offsetZ = ChunkCoord.y * ChunkSize;

            for (int x = 0; x <= ChunkSize; x++)
            {
                for (int z = 0; z <= ChunkSize; z++)
                {
                    int index = x * (ChunkSize + 1) + z;
                    verts[index] = new Vector3(x + offsetX, 0f, z + offsetZ);
                }
            }

            return verts;
        }

        int[] GenerateTriangles()
        {
            int[] tris = new int[ChunkSize * ChunkSize * 6];
            int t = 0;

            for (int x = 0; x < ChunkSize; x++)
            {
                for (int z = 0; z < ChunkSize; z++)
                {
                    int i0 = x * (ChunkSize + 1) + z;
                    int i1 = i0 + 1;
                    int i2 = i0 + (ChunkSize + 1);
                    int i3 = i2 + 1;

                    tris[t++] = i0;
                    tris[t++] = i3;
                    tris[t++] = i1;

                    tris[t++] = i0;
                    tris[t++] = i2;
                    tris[t++] = i3;
                }
            }

            return tris;
        }

        Vector2[] GenerateUVs()
        {
            Vector2[] uvs = new Vector2[_cachedVertices.Length];
            for (int x = 0; x <= ChunkSize; x++)
            {
                for (int z = 0; z <= ChunkSize; z++)
                {
                    int index = x * (ChunkSize + 1) + z;
                    uvs[index] = new Vector2((float)x / ChunkSize, (float)z / ChunkSize);
                }
            }
            return uvs;
        }

        void OnDestroy()
        {
            if (_nativeVertices.IsCreated)
                _nativeVertices.Dispose();
        }
    }
}