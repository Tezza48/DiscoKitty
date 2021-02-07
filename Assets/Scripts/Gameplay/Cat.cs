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
    public float minHandleSize = 0.5f;
    public float maxHandleSize = 1.5f;
    private float animSpeedMultiplier = 0.2f;
    private float animMoveThreshhold = 0.5f;

    [Header("ComponentRefs")]
    public Rigidbody2D _rigid;
    public CircleCollider2D touchHandle;
    public Collider2D catCollider;
    public LineRenderer _lineRenderer;
    public Transform laserTransform;
    public Animator animator;

#if UNITY_ANDROID || UNITY_IOS
    //private bool mIsFingerDragging;
    //private int mFingerIndex;

#endif
    private ECatState catState;

    // Use this for initialization
    void Start()
    {
        _lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Change the size of the the touc handle to match the camera's zoom
        CameraController cam = FindObjectOfType<CameraController>();
        float screenSize = Mathf.InverseLerp(cam.min + cam.buffer, cam.max + cam.buffer, cam.mCamera.orthographicSize);

        touchHandle.radius = Mathf.Lerp(minHandleSize, maxHandleSize, screenSize);

        if (_rigid.velocity.magnitude > animMoveThreshhold)
        {
            animator.SetBool("CatMoving", true);
            animator.speed = _rigid.velocity.magnitude * animSpeedMultiplier;
        }
        else
        {
            animator.SetBool("CatMoving", false);
            animator.speed = 1.0f;
        }
    }

    private void SetLineRendererPositions(Vector2 mouseWorldPos)
    {
        _lineRenderer.SetPosition(0, laserTransform.position + Vector3.back);
        _lineRenderer.SetPosition(1, (Vector3)mouseWorldPos + Vector3.back);
    }

#if UNITY_STANDALONE || true
    private void OnMouseDown()
    {
        _lineRenderer.enabled = true;
        SetLineRendererPositions(transform.position);
    }

    private void OnMouseDrag()
    {
        Vector2 targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (SettingsHelper.PointerDragOffsetEnabled)
        {
            targetPoint += new Vector2(0.0f, 1.0f);
        }

        //Debug.DrawLine(mouseWorldPos, transform.position);
        _rigid.AddForce((targetPoint - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
        SetLineRendererPositions(targetPoint);
    }

    private void OnMouseUp()
    {
        _lineRenderer.enabled = false;
    }
#endif

#if UNITY_ANDROID || UNITY_IOS
    void BeginTouchInput(Touch touch)
    {
        _lineRenderer.enabled = true;
        SetLineRendererPositions(transform.position);
        if (PlayerPrefs.GetInt("firstTouchLogged") == 0)
        {
            if (LevelManager.Singleton.currentLevelID == 0) // if this is the first level (should only be if the event hasnt been logged yet)
                FindObjectOfType<GameManager>().LogFirstCatTouch();
        }
    }
    void MovedTouchInput(Touch touch)
    {
        Vector2 targetPoint = Camera.main.ScreenToWorldPoint(touch.position);
        if (SettingsHelper.PointerDragOffsetEnabled)
        {
            targetPoint += new Vector2(0.0f, 1.0f);
        }

        //Debug.DrawLine(touchWorldPos, transform.position);
        _rigid.AddForce((targetPoint - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
        SetLineRendererPositions(targetPoint);
    }
    void StationaryTouchInput(Touch touch)
    {
        Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
        //Debug.DrawLine(touchWorldPos, transform.position);
        _rigid.AddForce((touchWorldPos - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
        SetLineRendererPositions(touchWorldPos);
    }
    void EndTouchInput(Touch touch)
    {
        _lineRenderer.enabled = false;
    }
#endif
}