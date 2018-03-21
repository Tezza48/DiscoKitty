using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public class Cat : MonoBehaviour
{
    public enum ECatState
    {
        Idle,
        Dragged
    }

    [Header("Settings")]
    public float dragForce = 4.0f;

    [Header("ComponentRefs")]
    public Rigidbody2D mRigid;
    public Collider2D mCollider;

#if UNITY_ANDROID || UNITY_IOS
    //private bool mIsFingerDragging;
    //private int mFingerIndex;

#endif
    private ECatState catState;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_STANDALONE
    private void OnMouseDown()
    {
        
    }
    private void OnMouseDrag()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(mouseWorldPos, transform.position);
        mRigid.AddForce((mouseWorldPos - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
    }
    private void OnMouseUp()
    {
        
    }
#endif

#if UNITY_ANDROID || UNITY_IOS
    void BeginTouchInput(Touch touch)
    {

    }

    void MovedTouchInput(Touch touch)
    {
        Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
        Debug.DrawLine(touchWorldPos, transform.position);
        mRigid.AddForce((touchWorldPos - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
    }

    void EndTouchInput(Touch touch)
    {

    }
#endif
}