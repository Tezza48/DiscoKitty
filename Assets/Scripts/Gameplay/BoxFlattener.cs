using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFlattener : MonoBehaviour
{
    public Sprite Built;
    public Sprite Flattened;

    public Collider2D boxCollider;
    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isFlat = false;
        foreach (var item in FindObjectsOfType<Cat>())
        {
            if (item.catCollider.IsTouching(boxCollider))
            {
                isFlat = true;
                break;
            }
        }
        spriteRenderer.sprite = isFlat ? Flattened : Built;

    }
}