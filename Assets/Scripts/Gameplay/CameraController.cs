using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float min = 5, max = 50;
    public float buffer = 2.0f;
    /*public float minBuffer = 1, maxBuffer = 10;*/

    [Range(0.1f, 10.0f)]
    public float lerpSpeed = 0.5f;

    public Camera mCamera;

    public bool isFixedAtCentre;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var tracked = CameraTracked.GetEnabledCameraTracked();

        if (tracked.Count == 0)
        {
            return;
        }

        var positions = tracked.Select(x =>
        {
            var pos = (Vector2)x.transform.position;
            return pos;
        });

        var centre = (isFixedAtCentre) ? Vector2.zero : positions.Aggregate((sum, next) => sum + next) / (tracked.Count);

        transform.position = Vector3.Lerp(transform.position, new Vector3(centre.x, centre.y, transform.position.z), Time.deltaTime);

        var furthest = positions
            .Select(x => (x - centre).magnitude)
            .Aggregate((acc, next) => Mathf.Max(acc, next));

        furthest = Mathf.Clamp(furthest, min, max);

        mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, furthest + buffer, Time.deltaTime);
    }
}
