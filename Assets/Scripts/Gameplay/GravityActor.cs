using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityActor : MonoBehaviour
{
    public bool canMove;

    Rigidbody2D _rigid;

    [Range(0.0f, 20.0f)]
    public float range = 5.0f;
    [Range(-20.0f, 20.0f)]
    public float forceAmmount = 1.0f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 force = new Vector2();
            float distance = 0.0f;
            foreach (GravityActor item in FindObjectsOfType<GravityActor>())
            {
                if (item != this)
                {
                    distance = Vector2.Distance(transform.position, item.transform.position);
                    if (distance < range)
                    {
                        force += (Vector2)(transform.position - item.transform.position) * item.forceAmmount;
                    }
                }
            }
            _rigid.AddForce(force, ForceMode2D.Force);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
