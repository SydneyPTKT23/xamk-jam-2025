using System.Collections;
using System.Collections.Generic;
using Terrain;
using UnityEngine;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField] SimplePlane plane;

    public int pos = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(plane.points[pos], 1f);
    }
}
