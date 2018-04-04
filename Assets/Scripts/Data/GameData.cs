using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

static class GameData
{
    const string FILENAME = "saveData";
    const string EXTENSION = ".json";

    public static GameSaveData LoadData()
    {
        string path = Application.persistentDataPath + "/" + FILENAME + EXTENSION;
        Debug.Log(path);
        if (File.Exists(path))
        {
            // load the data normally
            string json = File.ReadAllText(path);
            //Debug.Log("Loading\n" + json);
            GameSaveData data = new GameSaveData();

            data.FromSerialized(json);

            return data;
        }
        else
        {
            // make a new data file
            GameSaveData data = new GameSaveData();
            SaveData(data);
            return data;
        }
    }

    public static void SaveLevel(string name, LevelSaveData levelData)
    {
        GameSaveData data = LoadData();
        if (data.levelData.ContainsKey(name))
        {
            // if there's already a key, set the value
            data.levelData[name] = levelData;
        }
        else
        {
            // else set a new pair
            data.levelData.Add(name, levelData);
        }
        SaveData(data);
    }

    private static void SaveData(GameSaveData data)
    {
        string json = data.GetSerialized();
        //Debug.Log("Saving\n" + json);
        File.WriteAllText(Application.persistentDataPath + "/" + FILENAME + EXTENSION, json);
    }
}

class GameSaveData
{
    [Serializable]
    public class SerializableGameSaveData
    {
        // Need to use this class because unity will not serialize dictionaries

        public int highestLevel;
        public string[] names;
        public LevelSaveData[] levelDatas;

        public SerializableGameSaveData()
        {
            highestLevel = 0;
            names = new string[0];
            levelDatas = new LevelSaveData[0];
        }
    }

    public int highestLevel;
    public Dictionary<string, LevelSaveData> levelData;

    public GameSaveData()
    {
        highestLevel = 0;
        levelData = new Dictionary<string, LevelSaveData>();
    }

    public string GetSerialized()
    {
        SerializableGameSaveData serializable = new SerializableGameSaveData();
        serializable.highestLevel = highestLevel;
        serializable.names = levelData.Keys.ToArray();
        serializable.levelDatas = levelData.Values.ToArray();
        string json = JsonUtility.ToJson(serializable);
        return json;
    }

    public void FromSerialized(string json)
    {
        SerializableGameSaveData serialized = JsonUtility.FromJson<SerializableGameSaveData>(json);
        highestLevel = serialized.highestLevel;
        for (int i = 0; i < serialized.names.Length; i++)
        {
            levelData.Add(serialized.names[i], serialized.levelDatas[i]);
        }
    }
}

[Serializable]
class LevelSaveData
{
    public float completionTime;

    public LevelSaveData()
    {

    }

    public LevelSaveData(float completionTime)
    {
        this.completionTime = completionTime;
    }
}
