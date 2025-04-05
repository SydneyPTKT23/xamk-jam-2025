using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectFloat : MonoBehaviour
{
    [SerializeField] bool rotatable;
    [SerializeField] float rotateSpeed;

    void Update()
    {
        float s = Mathf.Sin(Time.time * 1);

        transform.position = new Vector3(transform.position.x, s, transform.position.z);

        if (rotatable)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }
}
