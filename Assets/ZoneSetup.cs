using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class ZoneSetup: MonoBehaviour
{
    [Range (1.0f, 10.0f)]
    public float size = 1.0f;

    public SpriteRenderer artwork;
    public CircleCollider2D circleCollider;

    public float[] sizes;
    public Vector2[] scaleRanges;
    public Sprite[] sprites;

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
        artwork.sprite = sprites[selectedSize];
        // Set the scale

        float scale = Mathf.Lerp(scaleRanges[selectedSize].x, scaleRanges[selectedSize].y, size / 10.0f);

        artwork.transform.localScale = new Vector3(scale, scale, scale);

        circleCollider.radius = size / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        SetupZone();
    }
}
#endif