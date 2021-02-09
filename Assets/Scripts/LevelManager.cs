using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance;

    public int currentLevelID;
    public string[] levels;

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
        Debug.Log("level: " + levelID + " " + levels[levelID]);

        // TODO WT: fix this. (errors when loading level from menu).
        SceneManager.LoadScene(levels[levelID]);
    }

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
