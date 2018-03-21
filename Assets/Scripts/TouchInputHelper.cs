using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHelper : MonoBehaviour
{
    private const int TOUCH_LAYER = 8;

#if UNITY_ANDROID || UNITY_IOS
    private Dictionary<int, GameObject> touchMap;
    private void Start()
    {
        touchMap = new Dictionary<int, GameObject>();
    }
#endif

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        GameObject obj;
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Collider2D coll = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position), 1 << TOUCH_LAYER);
                    if (coll != null)
                    {
                        touchMap.Add(touch.fingerId, coll.gameObject);
                        coll.SendMessage("BeginTouchInput", touch);
                    }
                    break;
                case TouchPhase.Moved:
                    if (touchMap.ContainsKey(touch.fingerId))
                    {
                        obj = touchMap[touch.fingerId];
                        obj.SendMessage("MovedTouchInput", touch);
                    }
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    if (touchMap.ContainsKey(touch.fingerId))
                    {
                        obj = touchMap[touch.fingerId];
                        //obj.SendMessage("EndTouchInput", touch);
                        touchMap.Remove(touch.fingerId);
                    }
                    break;
                case TouchPhase.Canceled:
                    if (touchMap.ContainsKey(touch.fingerId))
                    {
                        obj = touchMap[touch.fingerId];
                        //obj.SendMessage("CancelTouchInput", touch);
                        touchMap.Remove(touch.fingerId);
                    }
                    break;
                default:
                    break;
            }
        }
#endif
    }
}