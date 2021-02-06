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
        LevelDataArray levelData = new LevelDataArray();
        TextAsset levelJson = Resources.Load<TextAsset>(levels[currentLevelID]);
        levelData = JsonUtility.FromJson<LevelDataArray>(levelJson.ToString());

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

    internal IEnumerator StartNewLevel(LevelData[] data)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
            
        yield return new WaitUntil(() => load.isDone);

        FindObjectOfType<GameManager>().InstantiateLevel(data);
    }
}
