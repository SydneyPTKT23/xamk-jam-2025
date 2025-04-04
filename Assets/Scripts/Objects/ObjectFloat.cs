using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectFloat : MonoBehaviour
{

    float floatRange;
    float floatSpeed;

    void Update()
    {
        Vector3 upLimit = new Vector3(transform.position.x, transform.position.y + floatRange, transform.position.z);
        Vector3 downLimit = new Vector3(transform.position.x, transform.position.y - floatRange, transform.position.z);

        transform.position = Vector3.Lerp(upLimit, downLimit, floatSpeed);
    }
}
