using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class SessionLengthTracker : MonoBehaviour
{
    private SessionLengthTracker singleton;
    private float startTime;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!singleton)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
            Destroy(this);
        }
        startTime = Time.time;
    }

    private void OnApplicationQuit()
    {
        float elapsedTime = Time.time - startTime;
        FirebaseAnalytics.LogEvent("SessionEnded", "sessionTime", elapsedTime);
    }
}