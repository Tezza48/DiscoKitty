using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using UnityEngine.Analytics;

//[RequireComponent(typeof(Collider2D))]
public class GameManager : MonoBehaviour
{

    public enum ELevelState
    {
        Idle,
        AllIn,
        Success,
    }

    [Header("Component Referances")]
    public AudioSource mAudio;

    // Scene Referances
    private Zone[] zones;
    private Cat[] cats;

    // UI Referances
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
        // set the music to the vamp sound
        mAudio.clip = musVamp;
        // Set the vamp to the default tempo
        mAudio.pitch = musTempoIdle;

        tTimerCurrentHold = Time.time + holdTime;

        // Progress display on the UI
        UI_Progress = FindObjectOfType<UI_LevelProgress>();

        // Get referance to win screen on the UI
        UI_WinPanel = GameObject.Find("Win Panel");
        // Setup event to fire when the player clicks next level
        UI_WinPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(LoadNextLevel);
        // finally hide it until we need it later
        UI_WinPanel.SetActive(false);

        zones = FindObjectsOfType<Zone>();
        cats = FindObjectsOfType<Cat>();

        //howManyInText = GameObject.Find("HowManyInText").GetComponent<Text>();
        currentHoldTimeText = GameObject.Find("CurrentHoldText").GetComponent<Text>();

        // time that the user started the level in seconds. For analytics
        levelStartTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F12))
        {
            LevelComplete();
        }

        // determine how many of the cats are in
        int howManyIn = 0;
        foreach (Cat cat in cats)
        {
            bool wasCatIn = false;
            // is the cat in a zone
            foreach (Zone zone in zones)
            {
                if (wasCatIn)
                    continue;
                if (cat.catCollider.IsTouching(zone.zoneCollider))
                {
                    howManyIn++;
                    wasCatIn = true;
                }
            }
        }

        UI_Progress.CurrentCatsIn = howManyIn;

        if (levelState != ELevelState.Success)
        {
            if (howManyIn == cats.Length)
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

                break;
            case ELevelState.AllIn:

                currentHoldTimeText.text = "Hold for: " + (tTimerCurrentHold - Time.time).ToString("N1") + " Seconds";

                // lerp between the two pitches using current hold time
                mAudio.pitch = Mathf.Lerp(musTempoIdle, musTempoFinish, (holdTime - (tTimerCurrentHold - Time.time)) / holdTime);

                if (Time.time > tTimerCurrentHold)
                {
                    LevelComplete();
                }
                break;
            case ELevelState.Success:
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

        string levelName = SceneManager.GetActiveScene().name;
        float playTime = Time.time - levelStartTime;
        Analytics.CustomEvent("level_" + levelName, new Dictionary<string, object>
        {
            { "playTime", playTime}
        });
    }

    public void LoadNextLevel()
    {
        //Debug.Log("Trying to load next level");
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }
}
