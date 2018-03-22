using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using UnityEngine.Analytics;

[RequireComponent(typeof(Collider2D))]
public class GameManager : MonoBehaviour
{

    public enum ELevelState
    {
        Idle,
        AllIn,
        Success,
    }

    [Header("Component Referances")]
    public Collider2D mCollider;
    public SpriteRenderer mRenderer;
    public AudioSource mAudio;
    //public Repulsive mWinRepulsive;

    [Header("Zone Artwork")]
    public Sprite spriteIncomplete;
    public Sprite spriteComplete;

    //[Header("UI Elements")]
    //private Text howManyInText;
    private Text currentHoldTimeText;
    private GameObject UI_WinPanel;
    private UI_LevelProgress UI_Progress;

    [Tooltip("Time to keep all cats in to win")]
    public float holdTime = 3;

    [Header("Level Stuff")]
    public string nextLevel;

    [Header("Music")]
    public AudioClip musVamp;
    public AudioClip musTadaa;
    public float musTempoIdle = 1.0f;
    public float musTempoFinish = 2.0f;

    private float levelStartTime;

    private ELevelState levelState = ELevelState.Idle;
    //private bool areAllInside = false;
    private float tTimerCurrentHold;

    // Use this for initialization
    void Start()
    {
        mAudio.clip = musVamp;
        mAudio.pitch = musTempoIdle;
        tTimerCurrentHold = Time.time + holdTime;

        UI_Progress = FindObjectOfType<UI_LevelProgress>();

        UI_WinPanel = GameObject.Find("Win Panel");
        UI_WinPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(LoadNextLevel);

        UI_WinPanel.SetActive(false);
        
        //howManyInText = GameObject.Find("HowManyInText").GetComponent<Text>();
        currentHoldTimeText = GameObject.Find("CurrentHoldText").GetComponent<Text>();

        levelStartTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F12))
        {
            LevelComplete();
        }
        // count the number of cats inside
        int howManyIn = 0;
        bool allIn = true;
        // check that all repulsives are touching trigger
        Cat[] cats = FindObjectsOfType<Cat>();
        foreach (Cat item in cats)
        {
            if (!item.mCollider.IsTouching(mCollider))
            {
                allIn = false;
            }
            else
            {
                howManyIn++;
            }
        }

        UI_Progress.CurrentCatsIn = howManyIn;

        //howManyInText.text = "Cats in: " + howmanyin + "/" + cats.Length;

        if (levelState != ELevelState.Success)
        {
            if (allIn)
                levelState = ELevelState.AllIn;
            else
                levelState = ELevelState.Idle;
        }


        switch (levelState)
        {
            case ELevelState.Idle:
                tTimerCurrentHold = Time.time + holdTime;

                mAudio.pitch = musTempoIdle;

                currentHoldTimeText.text = "Hold for: " + (tTimerCurrentHold - Time.time).ToString("N1") + " Seconds";

                //mAudio.pitch = Mathf.Lerp(mAudio.pitch, musTempoIdle, 0.001f);

                mRenderer.sprite = spriteIncomplete;

                break;
            case ELevelState.AllIn:

                currentHoldTimeText.text = "Hold for: " + (tTimerCurrentHold - Time.time).ToString("N1") + " Seconds";

                // lerp between the two pitches using current hold time
                mAudio.pitch = Mathf.Lerp(musTempoIdle, musTempoFinish, (holdTime - (tTimerCurrentHold - Time.time))/ holdTime);

                mRenderer.sprite = spriteComplete;

                if (Time.time > tTimerCurrentHold)
                {
                    LevelComplete();
                }
                break;
            case ELevelState.Success:
                mRenderer.sprite = spriteComplete;
                break;
            default:
                break;
        }
    }

    private void LevelComplete()
    {
        //mWinRepulsive.enabled = true;
        UI_WinPanel.SetActive(true);
        levelState = ELevelState.Success;
        mAudio.clip = musTadaa;
        mAudio.pitch = 1.0f;
        mAudio.loop = false;
        mAudio.Play();
        Analytics.CustomEvent("levelCompleted", new Dictionary<string, object>
        {
            { "levelID", SceneManager.GetActiveScene().name },
            { "playTime", Time.time - levelStartTime}
        });
    }

    public void LoadNextLevel()
    {
        //Debug.Log("Trying to load next level");
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }
}
