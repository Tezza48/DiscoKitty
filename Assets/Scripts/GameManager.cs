using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public class GameManager : MonoBehaviour
{

    [Header("Component Referances")]
    public Collider2D mCollider;
    public SpriteRenderer mRenderer;
    public Repulsive mWinRepulsive;

    [Header("Zone Artwork")]
    public Sprite incomplete, complete;

    public bool areAllInside = false;

    [Header("UI Elements")]
    public Text howmanyInText, currentHoldTimeText;
    public GameObject UI_WinPanel;

    [Tooltip("Time to keep all cats in to win")]
    public float holdTime = 3;

    private float tTimerCurrentHold;

    // Use this for initialization
    void Start()
    {
        mWinRepulsive.enabled = false;
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

        mRenderer.sprite = areAllInside ? complete : incomplete;

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
        mWinRepulsive.enabled = true;
        UI_WinPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
