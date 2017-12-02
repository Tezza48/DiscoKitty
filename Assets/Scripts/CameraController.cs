using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int min = 5, max = 50;
    public int minBuffer = 1, maxBuffer = 10;

    [Range(0.1f, 10.0f)]
    public float lerpSpeed = 0.5f;

    public Camera mCamera;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // find furthest out cat
        int furthest = 0;
        foreach (var item in Repulsive.SpawnedRepulsives)
        {
            if (item.transform.position.magnitude > furthest)
            {
                furthest = (int)item.transform.position.magnitude;
            }
        }

        if (furthest < min)
        {
            furthest = min;
        }
        else if (furthest > max)
        {
            furthest = max;
        }

        // add correct buffer
        float alpha = furthest / (float)max;
        Debug.Log(alpha);
        float buffer = Mathf.Lerp(minBuffer, maxBuffer, alpha);


        mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, furthest + buffer, Time.deltaTime);

    }
}
