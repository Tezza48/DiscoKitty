using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
    public class LevelDataArray
    {
        public LevelEntity[] Content;

        public LevelDataArray()
        {
            Content = new LevelEntity[0];
        }

        public LevelDataArray(LevelEntity[] data)
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

    internal IEnumerator StartNewLevel(LevelEntity[] data)
    {
		AsyncOperation load = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
		Debug.LogWarning("Function Currently not Operational due to refactoring");
		yield return new WaitUntil(() => load.isDone);

        //FindObjectOfType<GameManager>().InstantiateLevel(data);
    }
}
