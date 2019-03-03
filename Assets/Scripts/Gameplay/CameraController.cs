using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isStationary = true;
    public float min = 5, max = 50;
    public float buffer = 2.0f;
    /*public float minBuffer = 1, maxBuffer = 10;*/

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
        var tracked = CameraTracked.GetEnabledCameraTracked();

        // find the average position it not stationary
        Vector2 avereagePosition = Vector2.zero;
        foreach (var item in tracked)
            avereagePosition += (Vector2)item.transform.position;
        avereagePosition /= tracked.Count;
            
        if (!isStationary)
            transform.position = new Vector3(avereagePosition.x, avereagePosition.y, transform.position.z);


        // find furthest out tracked item
        float furthest = 0.0f;
        foreach (var item in tracked)
        {
            if (item.transform.position.magnitude > furthest)
            {
                furthest = ((Vector2)item.transform.position - (Vector2)transform.position).magnitude;
            }
        }
        furthest = Mathf.Clamp(furthest, min, max);

        // add correct buffer
        float alpha = furthest / (float)max;
        mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, furthest + buffer, Time.deltaTime);
    }
}
