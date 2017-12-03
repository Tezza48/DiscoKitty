using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CatAnimation : MonoBehaviour
{
    //[Range(0.0f, 10.0f)]
    private float speed = 1.0f;

    public Rigidbody2D mRigid;
    public AudioSource mAudioSource;

    public Transform[] legs, feet;
    public Vector2[] directions;

    public Vector2 offsetFoot, offsetLeg;

    public float legDistance = 0.5f, footDistance = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            Vector2 rootPos = Vector2.Scale(offsetLeg, directions[i]);
            float alpha = Mathf.Sin(Time.time * mRigid.velocity.magnitude * speed + i) / 2.0f + 0.5f;
            legs[i].transform.localPosition = Vector2.Lerp(rootPos, rootPos + directions[i] * legDistance, alpha);

            rootPos = Vector2.Scale(offsetFoot, directions[i]);
            feet[i].transform.localPosition = Vector2.Lerp(rootPos, rootPos + directions[i] * footDistance, alpha);
        }
    }
}
