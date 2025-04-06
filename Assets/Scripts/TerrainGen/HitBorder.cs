using FaS.DiverGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terrain;
using UnityEngine;
using UnityEngine.UIElements;

public class HitBorder : MonoBehaviour
{
    [SerializeField] SimplePlane plane;

    [Range(0f, 1f)]
    [SerializeField] float t;

    [SerializeField] float speed;

    [SerializeField] GameObject Player;

    public List<Collider> ColliderList;

    void Awake()
    {
        ColliderList = GetComponentsInChildren<Collider>().ToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(StartReset());
        SoundsOnPlayer.PlaySound(SoundType.AMB_1, 1);
    }


    private IEnumerator StartReset()
    {
        float countDown = 1f;
        while (countDown >= 0)
        {
            
            float warp = Mathf.Lerp(0, 10, t) * Time.deltaTime;
            speed = Mathf.Lerp(0, 10, t);

            for (int j = 0; j < plane.noiseSettings.Count; j++)
            {
                plane.noiseSettings[j].Amplitude += warp * speed;
                plane.noiseSettings[j].Frequency += warp * speed;

            }
            countDown -= Time.smoothDeltaTime;
            yield return null;
        }
        Player.transform.position = new Vector3(1024, 5, 1024);
        StartCoroutine(EndReset());
    }

    private IEnumerator EndReset()
    {
        float countDown = 1f;
        while (countDown >= 0)
        {
            float warp = Mathf.Lerp(0, 10, t) * Time.deltaTime;
            speed = Mathf.Lerp(0, 10, t);

            for (int j = 0; j < plane.noiseSettings.Count; j++)
            {
                plane.noiseSettings[j].Amplitude -= warp * speed;               
                plane.noiseSettings[j].Frequency -= warp * speed;               
            }
            countDown -= Time.smoothDeltaTime;
            yield return null;
        }
        plane.noiseSettings[0].Amplitude = 4.45f; 
        plane.noiseSettings[0].Frequency = 0.007f; 

        plane.noiseSettings[1].Amplitude = 2.37f; 
        plane.noiseSettings[1].Frequency = 0.019f; 
    }
}
