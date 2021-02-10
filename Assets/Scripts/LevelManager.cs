using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance;

    public int currentLevelID;
    public LevelList levels;

    public static LevelManager Instance { get => instance; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    internal void LoadLevel(int levelID)
    {
        currentLevelID = levelID;
        Debug.Log("level: " + levelID + " " + levels.levels[levelID]);

        // TODO WT: fix this. (errors when loading level from menu).
        SceneManager.LoadScene(levels.levels[levelID]);
    }

    internal void LoadNextLevel()
    {
        if (currentLevelID+1 == levels.levels.Length)
        {
            // dont do anything if there isnt a next level
            return;
        }
        LoadLevel(currentLevelID + 1);
    }
}
