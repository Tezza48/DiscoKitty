using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
        // find furthest out cat
        float furthest = 0.0f;
        foreach (var item in FindObjectsOfType<CameraTracked>())
        {
            if (item.transform.position.magnitude > furthest)
            {
                furthest = (int)item.transform.position.magnitude;
            }
        }

        furthest = Mathf.Clamp(furthest, min, max);


        mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, furthest + buffer, Time.deltaTime);

    }
}
