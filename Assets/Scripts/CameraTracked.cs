using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracked : MonoBehaviour
{

    private static List<CameraTracked> EnabledObjects;

    public static List<CameraTracked> GetEnabledCameraTracked()
    {
        return EnabledObjects;
    }

    private void Awake()
    {
        if (EnabledObjects == null)
        {
            EnabledObjects = new List<CameraTracked>();
        }
    }

    private void OnEnable()
    {
        EnabledObjects.Add(this);
    }

    private void OnDisable()
    {
        EnabledObjects.Remove(this);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
