using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum ObjectType
    {
        Cat,
        Pickle,
        Zone,
        Box,
        None
    }

    [System.Serializable]
    public class LevelList
    {
        public string[] Levels;
        public LevelList()
        {
            Levels = new string[0];
        }
    }

    [Serializable]
    public class LevelEntity
    {
        public Vector3 PositionAndRotation;
        public ObjectType Type;
        public float Radius;
    }

    [Serializable]
    public class CameraInfo
    {
        public bool fixedAtCentre;
        public float min;
        public float max;
    }

    [System.Serializable]
    public class LevelData
    {
        public CameraInfo cameraInfo;
        public LevelEntity[] Content;

        public LevelData()
        {
            Content = new LevelEntity[0];
        }

        public LevelData(LevelEntity[] data)
        {
            Content = data;
        }
    }

    static LevelManager singleton;

    public int currentLevelID;
    public string[] levels;

    public static LevelManager Singleton { get { return singleton; } }

    private void Awake()
    {
        // Setup Singleton
        DontDestroyOnLoad(gameObject);
        if (!singleton)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Load level list from json file
        LevelList levelList = new LevelList();
        TextAsset levelsJson = Resources.Load<TextAsset>("levels");
        JsonUtility.FromJsonOverwrite(levelsJson.ToString(), levelList);
        levels = levelList.Levels;
    }

    internal void LoadLevel(int levelID)
    {
        Debug.Log("level: " + levelID + " " + levels[levelID]);
        currentLevelID = levelID;
        LevelData levelData = new LevelData();
        TextAsset levelJson = Resources.Load<TextAsset>(levels[currentLevelID]);
        levelData = JsonUtility.FromJson<LevelData>(levelJson.ToString());

        StartCoroutine(StartNewLevel(levelData.Content));
    }

    //internal int GetIDfromName()

    internal void LoadNextLevel()
    {
        if (currentLevelID+1 == levels.Length)
        {
            // dont do anything if there isnt a next level
            return;
        }
        LoadLevel(currentLevelID + 1);
    }

    internal IEnumerator StartNewLevel(LevelEntity[] data)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
            
        yield return new WaitUntil(() => load.isDone);

        FindObjectOfType<GameManager>().InstantiateLevel(data);
    }
}
