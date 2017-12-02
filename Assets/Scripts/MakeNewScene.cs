using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MakeNewScene : MonoBehaviour
{

    public GameObject[] mDefaultObjects;

    private void Awake()
    {
        foreach (var item in mDefaultObjects)
        {
            Instantiate(item);
        }
        DestroyImmediate(gameObject);
    }

}
