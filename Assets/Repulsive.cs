using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public class Repulsive : MonoBehaviour
{
    public enum ECatState
    {
        Idle,
        Dragged
    }

    private static List<Repulsive> sSpawnedRepulsives;

    [Header("Settings")]
    public float repellRange = 1.0f;
    public float repellforce = 1.0f;
    public float dragForce = 4.0f;

    [Header("ComponentRefs")]
    public Rigidbody2D mRigid;
    public Collider2D mCollider;

    private ECatState catState;

    public static List<Repulsive> SpawnedRepulsives
    {
        get { return sSpawnedRepulsives; } set { sSpawnedRepulsives = value; }
    }

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

}