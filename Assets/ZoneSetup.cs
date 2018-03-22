using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class ZoneSetup: MonoBehaviour
{
    public bool isOnUpdate = false;
    [Range (1.0f, 10.0f)]
    public float size = 1.0f;

    GameManager manager;

    public SpriteRenderer artwork;
    public CircleCollider2D circleCollider;

    public float[] sizes;
    public Vector2[] scaleRanges;
    public Sprite[] zoneEmpty;
    public Sprite[] zoneFull;

    private void Awake()
    {
    }

    private void Start()
    {
        if (EditorApplication.isPlaying)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }

    private void OnEnable()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        manager = GetComponent<GameManager>();
        SetupZone();
    }

    // Use this for initialization
    void SetupZone()
    {
        int selectedSize = sizes.Length - 1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (size < sizes[i])
            {
                selectedSize = i;
                break;
            }
        }

        // Set the artwork
        artwork.sprite = zoneEmpty[selectedSize];

        manager.spriteIncomplete = zoneEmpty[selectedSize];
        manager.spriteComplete = zoneFull[selectedSize];

        // Set the scale

        float scale = Mathf.Lerp(scaleRanges[selectedSize].x, scaleRanges[selectedSize].y, size / 10.0f);

        artwork.transform.localScale = new Vector3(scale, scale, scale);

        circleCollider.radius = size / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnUpdate)
        {
            SetupZone();
        }
    }
}
#endif