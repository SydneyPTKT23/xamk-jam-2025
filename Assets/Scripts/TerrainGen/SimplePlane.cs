using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class SimplePlane : MonoBehaviour
{
    //Noise Layers
    public List<NoiseSettings> noiseSettings = new List<NoiseSettings>();


    [Range(1f, 10000f)]

    public float Size = 1000.0f;

    //[Range(0.001f, 1f)]
    //public float noiseFrq;

    //[Range(1, 300)]
    //public float Amp;

    [Range(10,255)]
    public int Segments = 100;

    public bool UseThreshold = false;

    //[Range(0f, -100f)]
    public float xshift = 0.0f;
    //[Range(0f, -100f)]
    public float yshift = 0.0f;

    private Mesh mesh;

    private void OnDrawGizmos()
    {
        GenerateMesh();

        //float delta = Size / (Segments - 1);

        //for (int i = 0; i < Segments; i++)
        //{
        //    for (int j = 0; j < Segments; j++)
        //    {
        //        float x = i * delta;
        //        float y = j * delta;
        //        float noise = Mathf.PerlinNoise(x, y);

        //        Gizmos.color = Color.red;
        //        Gizmos.DrawSphere(new Vector3(x, noise, y), 0.2f);

        //    }
        //}
    }

    void GenerateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Terrain Mesh";
        }
        else { mesh.Clear(); }

        List<Vector3> vertices = new();
        List<int> tris = new();
        List<Vector2> uvs = new();


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
                for (int k= 0; k < noiseSettings.Count; k++)
                {
                    noiseValue += noiseSettings[k].Amplitude * 2.0f *
                                    (Mathf.PerlinNoise((x+xshift) * noiseSettings[k].Frequency,
                                                       (y+yshift) * noiseSettings[k].Frequency) - 0.5f);
                }

                if (UseThreshold)
                {
                    if ((noiseValue < 0.0f))
                    {
                        noiseValue = 0.0f;
                    }
                }
                //float height = noiseValue * Amp;

                vertices.Add(new Vector3(x, noiseValue, y));
                uvs.Add(new Vector2((x + xshift) / Size, (y + yshift) / Size));

            }
        }
        //Generate triangles
        for (int i = 0;i < Segments-1; i++)
        {
            for (int j = 0;j < Segments-1; j++)
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

    private void Update()
    {
        GenerateMesh();
        xshift += 0.01f;
        yshift += 0.01f;
    }
}
