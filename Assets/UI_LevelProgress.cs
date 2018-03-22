using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelProgress : MonoBehaviour
{
    int totalCats;
    int currentCatsIn;

    public GameObject progressPrefab;
    public Image[] progressImages;

    public Sprite inactive;
    public Sprite active;

    public int CurrentCatsIn { set { currentCatsIn = value; } }

    // Use this for initialization
    void Start()
    {
        totalCats = FindObjectsOfType<Cat>().Length;
        progressImages = new Image[totalCats];
        for (int i = 0; i < totalCats; i++)
        {
            progressImages[i] = Instantiate(progressPrefab, transform).GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < totalCats; i++)
        {
            progressImages[i].sprite = i < currentCatsIn ? active : inactive;
        }
    }
}