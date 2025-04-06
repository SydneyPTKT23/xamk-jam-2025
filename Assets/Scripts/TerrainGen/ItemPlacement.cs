using FaS.DiverGame.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField] SimplePlane plane;
    [SerializeField] GameObject GameObject;

    public int pos = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(plane.points[pos], 1f);

    }

    private void Update()
    {
        //Vector3 tpos = plane.points[pos];
        //GameObject.transform.position = tpos;
        //Debug.Log(tpos);
    }
}
