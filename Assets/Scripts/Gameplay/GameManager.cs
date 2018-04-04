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
    // Level Loading Stuff
    public enum ObjectType
    {
        Cat,
        Pickle,
        Zone,
        None
    }

    [System.Serializable]
    public class LevelData
    {
        public Vector3 PositionAndRotation;
        public ObjectType Type;
        public float Radius;
    }

    [System.Serializable]
    public class LevelDataArray
    {
        public LevelData[] Content;

        public LevelDataArray()
        {
            Content = new LevelData[0];
        }

        public LevelDataArray(LevelData[] data)
        {
            Content = data;
        }
    }

    // General Stuff
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
    public Text UI_WinScreenTime;

    [Tooltip("Time to keep all cats in to win")]
    public float holdTime = 3;

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

        // These are only useful when the scene is already populates when loaded
        zones = FindObjectsOfType<Zone>();
        cats = FindObjectsOfType<Cat>();

        //howManyInText = GameObject.Find("HowManyInText").GetComponent<Text>();
        currentHoldTimeText = GameObject.Find("CurrentHoldText").GetComponent<Text>();

        // time that the user started the level in seconds. For analytics
        levelStartTime = Time.time;

    }

    internal void InstantiateLevel(LevelManager.LevelData[] data)
    {
        // destroy all level object just incase the blank level was saved over
        GameObject[] LevelObjects = GameObject.FindGameObjectsWithTag("LevelContent");

        foreach (var item in LevelObjects)
        {
            DestroyImmediate(item);
        }

        foreach (var item in data)
        {
            GameObject newObject;

            Vector2 pos = item.PositionAndRotation;
            float rotation = item.PositionAndRotation.z;
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, rotation);
            newObject = GetComponent<LevelDataPrefabs>().Prefabs[(int)item.Type];

            newObject = Instantiate(newObject, pos, rot);

            if (item.Type == LevelManager.ObjectType.Zone)
            {
                newObject.GetComponent<Zone>().size = item.Radius;
                newObject.GetComponent<Zone>().SetupZone();
            }
        }
        zones = FindObjectsOfType<Zone>();
        cats = FindObjectsOfType<Cat>();
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

                currentHoldTimeText.text = "" + (tTimerCurrentHold - Time.time).ToString("N1") + "";

                break;
            case ELevelState.AllIn:

                currentHoldTimeText.text = "" + (tTimerCurrentHold - Time.time).ToString("N1") + "";

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

    //public void SetLevel(string name, bool clear = false)
    //{
    //    if (clear)
    //    {
    //        GameObject[] LevelObjects = GameObject.FindGameObjectsWithTag("LevelContent");
    //
    //        foreach (var item in LevelObjects)
    //        {
    //            DestroyImmediate(item);
    //        }
    //    }
    //
    //    LevelDataArray levelData = new LevelDataArray();
    //    TextAsset json = Resources.Load<TextAsset>(name);
    //    JsonUtility.FromJsonOverwrite(json.ToString(), levelData);
    //
    //    LevelDataPrefabs dataPrefabs = GetComponent<LevelDataPrefabs>();
    //
    //    foreach (var item in levelData.Content)
    //    {
    //        GameObject newObject;
    //
    //        Vector2 pos = item.PositionAndRotation;
    //        float rotation = item.PositionAndRotation.z;
    //        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, rotation);
    //        newObject = dataPrefabs.Prefabs[(int)item.Type];
    //
    //        newObject = Instantiate(newObject, pos, rot);
    //
    //        if (item.Type == ObjectType.Zone)
    //        {
    //            newObject.GetComponent<Zone>().size = item.Radius;
    //        }
    //    }
    //}

    private void LevelComplete()
    {
        //mWinRepulsive.enabled = true;
        UI_WinPanel.SetActive(true);
        levelState = ELevelState.Success;
        mAudio.clip = musTadaa;
        mAudio.pitch = 1.0f;
        mAudio.loop = false;
        mAudio.Play();

        string levelName = LevelManager.Singleton.levels[LevelManager.Singleton.currentLevelID];
        float playTime = Time.time - levelStartTime;

        UI_WinScreenTime.text = playTime.ToString("N2");

        // Currently dont cate about the highest level completed
        GameData.SaveLevel(levelName, new LevelSaveData(playTime));

        //Analytics.CustomEvent("level_" + levelName, new Dictionary<string, object>
        //{
        //    { "playTime", playTime}
        //});
    }

    public void LoadNextLevel()
    {
        //Debug.Log("Trying to load next level");
        LevelManager.Singleton.LoadNextLevel();
    }
}
