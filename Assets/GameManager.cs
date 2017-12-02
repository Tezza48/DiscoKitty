using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GameManager : MonoBehaviour
{

    public Collider2D mCollider;

    public bool areAllInside = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check that all repulsives are touching trigger
        foreach (Repulsive item in Repulsive.SpawnedRepulsives)
        {
            if (!item.mCollider.IsTouching(mCollider))
            {
                areAllInside = false;
                break;
            }
            else
            {
                areAllInside = true;
            }
        }
        Debug.Log("All Are In? " + areAllInside);
    }
}
