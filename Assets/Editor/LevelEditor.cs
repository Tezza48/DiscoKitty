using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using LevelData = LevelManager.LevelData;
using LevelDataArray = LevelManager.LevelDataArray;
using ObjectType = LevelManager.ObjectType;

public class LevelEditor : EditorWindow
{


    //[System.Serializable]
    //public class LevelListArray
    //{
    //    public string[] names;

    //    public LevelListArray()
    //    {
    //        names = new string[0];
    //    }
    //}

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

    int toolbarTab = 0;

    // Level Editor
    TextAsset levelFile;
    private string fileName = "";

    // Level Order
    TextAsset levelsFile;
    int numLevels = 0;
    TextAsset[] levels = new TextAsset[0];

    private void OnGUI()
    {
        toolbarTab = GUILayout.Toolbar(toolbarTab, new string[] { "Level Editor", "Level Order" });

        switch (toolbarTab)
        {
            case 0:
                #region New File
                EditorGUILayout.LabelField("New File");

                EditorGUILayout.BeginHorizontal();

                bool createNewFile = GUILayout.Button("Create New File", GUILayout.MaxWidth(100.0f));
                EditorGUIUtility.labelWidth = 70.0f;
                fileName = EditorGUILayout.TextField("Filename:", fileName, GUILayout.ExpandWidth(true));

                if (createNewFile)
                {
                    LevelDataArray data = new LevelDataArray();
                    string json = EditorJsonUtility.ToJson(data); string finalFile = "Assets/Resources/" + fileName + ".json";

                    using (FileStream fs = new FileStream(finalFile, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }
                    AssetDatabase.Refresh();
                }

                EditorGUILayout.EndHorizontal();
                #endregion

                EditorGUILayout.Space();

                #region File Operations
                EditorGUILayout.LabelField("File Operations");

                levelFile = (TextAsset)EditorGUILayout.ObjectField(levelFile, typeof(TextAsset), true);


                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save"))
                {
                    GameObject[] LevelObjects = GameObject.FindGameObjectsWithTag("LevelContent");
                    List<LevelData> levelData = new List<LevelData>();            

                    foreach (var item in LevelObjects)
                    {
                        LevelData itemData = new LevelData();
                        Zone zone = item.GetComponent<Zone>();
                        Cat cat = item.GetComponent<Cat>();
                        BoxFlattener box = item.GetComponent<BoxFlattener>();
                        if (zone)
                        {
                            itemData.Type = ObjectType.Zone;
                            itemData.Radius = zone.size;
                        }
                        else if (cat)
                        {
                            itemData.Type = ObjectType.Cat;
                        }
                        else if (box)
                        {
                            itemData.Type = ObjectType.Box;
                        }
                        else
                        {
                            itemData.Type = ObjectType.Pickle;
                        }

                        Transform trans = item.GetComponent<Transform>();

                        if ( trans )
                        {
                            itemData.PositionAndRotation = trans.position;
                            itemData.PositionAndRotation.z = trans.eulerAngles.z;
                        }

                        levelData.Add(itemData);
                    }

                    LevelDataArray levelDataArray = new LevelDataArray(levelData.ToArray());

                    string json = EditorJsonUtility.ToJson(levelDataArray, true);

                    string finalFile = "Assets/Resources/" + levelFile.name + ".json";

                    using (FileStream fs = new FileStream(finalFile, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }

                    UnityEditor.AssetDatabase.Refresh();
                }

                if ( GUILayout.Button("Load"))
                {
                    GameObject[] LevelObjects = GameObject.FindGameObjectsWithTag("LevelContent");

                    foreach(var item in LevelObjects)
                    {
                        DestroyImmediate(item);
                    }

                    //string finalFile = "Assets/Resources/" + fileName;

                    LevelDataArray levelData = new LevelDataArray();
                    TextAsset json = levelFile;
                    EditorJsonUtility.FromJsonOverwrite(json.ToString(), levelData);

                    LevelDataPrefabs dataPrefabs = GameObject.Find("GameManager").GetComponent<LevelDataPrefabs>();

                    foreach (var item in levelData.Content)
                    {
                        GameObject newObject;

                        Vector2 pos = item.PositionAndRotation;
                        float rotation = item.PositionAndRotation.z;
                        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, rotation);
                        newObject = dataPrefabs.Prefabs[(int)item.Type];

                        newObject = Instantiate(newObject, pos, rot);

                        if (item.Type == ObjectType.Zone)
                        {
                            newObject.GetComponent<Zone>().size = item.Radius;
                            newObject.GetComponent<Zone>().SetupZone();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                #endregion

                break;
            case 1:

                levelsFile = (TextAsset)EditorGUILayout.ObjectField(levelsFile, typeof(TextAsset), true);


                EditorGUILayout.BeginHorizontal();
                bool doSave = GUILayout.Button("Save");
                bool doLoad = GUILayout.Button("Load");
                EditorGUILayout.EndHorizontal();


                numLevels = EditorGUILayout.IntField("Levels", numLevels);
                if (numLevels > levels.Length)
                {
                    Array.Resize(ref levels, numLevels);
                }

                if (numLevels <= 0)
                {
                    numLevels = 1;
                }

                for (int i = 0; i < numLevels; i++)
                {
                    levels[i] = (TextAsset)EditorGUILayout.ObjectField(levels[i], typeof(TextAsset), true);
                }

                if (doSave)
                {
                    LevelManager.LevelList levelList = new LevelManager.LevelList();
                    levelList.Levels = new string[numLevels];
                    for (int i = 0; i < numLevels; i++)
                    {
                        levelList.Levels[i] = levels[i].name;
                    }


                    // Save it to json
                    string json = EditorJsonUtility.ToJson(levelList, true);

                    string finalFile = "Assets/Resources/" + levelsFile.name + ".json";

                    using (FileStream fs = new FileStream(finalFile, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }

                    UnityEditor.AssetDatabase.Refresh();
                }

                if (doLoad)
                {
                    LevelManager.LevelList levelList = new LevelManager.LevelList();
                    EditorJsonUtility.FromJsonOverwrite(levelsFile.ToString(), levelList);

                    numLevels = levelList.Levels.Length;
                    levels = new TextAsset[numLevels];
                    for (int i = 0; i < numLevels; i++)
                    {
                        levels[i] = Resources.Load<TextAsset>(levelList.Levels[i]);
                    }
                }

                break;
            default:
                break;
        }
    }
}
