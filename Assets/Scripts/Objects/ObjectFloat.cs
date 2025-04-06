using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectFloat : MonoBehaviour
{
    [SerializeField] bool rotatable;
    [SerializeField] float rotateSpeed;

    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset = new();
    private Vector3 tempPos = new();

    private void Start()
    {
        posOffset = transform.position;
    }

    private void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;

        if (rotatable)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }
}
