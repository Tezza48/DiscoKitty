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
    }

    internal void LoadLevel(int levelID)
    {
        Debug.Log("level: " + levelID + " " + levels[levelID]);

        SceneManager.LoadScene(SceneManager.GetSceneByPath(levels[levelID]).buildIndex);
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
}
