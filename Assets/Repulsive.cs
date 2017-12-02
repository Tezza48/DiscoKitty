using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsive : MonoBehaviour
{

    public static List<Repulsive> sSpawnedRepulsives;

    public float repellRange = 1.0f;
    public float repellforce = 1.0f;

    [Header("ComponentRefs")]
    public Rigidbody2D mRigid;

    private void Awake()
    {
        if (sSpawnedRepulsives == null)
        {
            sSpawnedRepulsives = new List<Repulsive>();
        }
    }
    private void OnEnable()
    {
        sSpawnedRepulsives.Add(this);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 force = new Vector2();
        foreach (Repulsive item in sSpawnedRepulsives)
        {
            if (item != this)
            {
                if (Vector2.Distance(transform.position, item.transform.position) < repellRange)
                {
                    force += (Vector2)(transform.position - item.transform.position);

                }
            }
        }
        mRigid.AddForce(force * repellforce, ForceMode2D.Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, repellRange);
    }

}