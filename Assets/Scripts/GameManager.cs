using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

[RequireComponent(typeof(Collider2D))]
public class GameManager : MonoBehaviour
{

    [Header("Component Referances")]
    public Collider2D mCollider;
    public SpriteRenderer mRenderer;
    //public Repulsive mWinRepulsive;

    [Header("Zone Artwork")]
    public Sprite spriteIncomplete;
    public Sprite spriteComplete;

    [Header("UI Elements")]
    public Text howmanyInText, currentHoldTimeText;
    public GameObject UI_WinPanel;

    [Tooltip("Time to keep all cats in to win")]
    public float holdTime = 3;

    [Header("Level Stuff")]
    public string nextLevel;

    private bool areAllInside = false;
    private float tTimerCurrentHold;

    // Use this for initialization
    void Start()
    {
        tTimerCurrentHold = Time.time + holdTime;
    }

    // Update is called once per frame
    void Update()
    {
        int howmanyin = 0;
        bool areAllInside = true;
        // check that all repulsives are touching trigger
        foreach (Repulsive item in Repulsive.SpawnedRepulsives)
        {
            if (!item.mCollider.IsTouching(mCollider))
            {
                areAllInside = false;
            }
            else
            {
                howmanyin++;
            }
        }
        Debug.Log("All Are In? " + areAllInside);

        howmanyInText.text = "Cats in: " + howmanyin + "/" + Repulsive.SpawnedRepulsives.Count; 

        mRenderer.sprite = areAllInside ? spriteComplete : spriteIncomplete;

        if (areAllInside)
        {
            if (Time.time > tTimerCurrentHold)
            {
                LevelComplete();
            }
        }
        else
        {
            tTimerCurrentHold = Time.time + holdTime;
        }
        
        currentHoldTimeText.text = "Hold for: " + (tTimerCurrentHold - Time.time).ToString("N1") + " Seconds";
        

    }

    private void LevelComplete()
    {
        //mWinRepulsive.enabled = true;
        UI_WinPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Trying to load next level");
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }
}
