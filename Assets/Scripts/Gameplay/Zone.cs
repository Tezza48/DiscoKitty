using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Zone: MonoBehaviour
{
    [Header("Setup")]
    public bool isOnUpdate = false;
    [Range (1.0f, 10.0f)]
    public float size = 1.0f;

    public SpriteRenderer artwork;
    public CircleCollider2D zoneCollider;

    public float[] sizes;
    public Vector2[] scaleRanges;
    public Sprite[] zoneEmpty;
    public Sprite[] zoneFull;

    private int zoneSize;

    private int howManyIn;

    public int HowManyIn { get { return howManyIn; } }

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        zoneCollider = GetComponent<CircleCollider2D>();
        SetupZone();
    }

    // Use this for initialization
    void SetupZone()
    {
        zoneSize = sizes.Length - 1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (size < sizes[i])
            {
                zoneSize = i;
                break;
            }
        }

        // Set the artwork
        artwork.sprite = zoneEmpty[zoneSize];

        //manager.spriteIncomplete = zoneEmpty[selectedSize];
        //manager.spriteComplete = zoneFull[selectedSize];

        // Set the scale

        float scale = Mathf.Lerp(scaleRanges[zoneSize].x, scaleRanges[zoneSize].y, size / 10.0f);

        artwork.transform.localScale = new Vector3(scale, scale, scale);

        zoneCollider.radius = size / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnUpdate)
        {
            SetupZone();
        }

        howManyIn = 0;

        foreach (Cat cat in FindObjectsOfType<Cat>())
        {
            if (cat.catCollider.IsTouching(zoneCollider))
            {
                howManyIn++;
            }
        }

        if (howManyIn > 0)
        {
            artwork.sprite = zoneFull[zoneSize];
        }
        else
        {
            artwork.sprite = zoneEmpty[zoneSize];
        }
    }
}