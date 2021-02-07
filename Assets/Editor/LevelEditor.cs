using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using LevelEntity = LevelManager.LevelEntity;
using LevelData = LevelManager.LevelData;
using ObjectType = LevelManager.ObjectType;

public class LevelEditor : EditorWindow
{
    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

    int toolbarTab = 0;

    // Level Editor
    private string editor_NewFilename = "";
    TextAsset editor_LevelFile;

    // Level Order
    TextAsset order_LevelsFile;
    int order_NumLevels = 0;
    TextAsset[] order_LevelFiles = new TextAsset[0];
    Vector2 order_ScrollPos = new Vector2();

    private void OnGUI()
    {
        toolbarTab = GUILayout.Toolbar(toolbarTab, new string[] { "Level Editor", "Level Order" });

        switch (toolbarTab)
        {
            case 0: // Level Editor
                #region New File
                EditorGUILayout.LabelField("New File");

                EditorGUILayout.BeginHorizontal();
                bool createNewFile = GUILayout.Button("Create New File", GUILayout.MaxWidth(100.0f));

                EditorGUIUtility.labelWidth = 70.0f;
                editor_NewFilename = EditorGUILayout.TextField("Filename:", editor_NewFilename, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                #endregion

                EditorGUILayout.Space();

                #region File Operations
                EditorGUILayout.LabelField("File Operations");

                editor_LevelFile = (TextAsset)EditorGUILayout.ObjectField(editor_LevelFile, typeof(TextAsset), true);

                EditorGUILayout.BeginHorizontal();
                bool editor_doSave = GUILayout.Button("Save");
                bool editor_doLoad = GUILayout.Button("Load");
                EditorGUILayout.EndHorizontal();

                if (createNewFile)
                {
                    LevelData data = new LevelData();
                    string json = EditorJsonUtility.ToJson(data); string finalFile = "Assets/Resources/" + editor_NewFilename + ".json";

                    using (FileStream fs = new FileStream(finalFile, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }
                    AssetDatabase.Refresh();

                    // Put new file in box
                    editor_LevelFile = Resources.Load<TextAsset>(editor_NewFilename);
                    // Load the new File
                    editor_doLoad = true;
                }

                if (editor_doSave)
                {
                    GameObject[] LevelObjects = GameObject.FindGameObjectsWithTag("LevelContent");
                    List<LevelEntity> entities  = new List<LevelEntity>();

                    foreach (var item in LevelObjects)
                    {
                        LevelEntity itemData = new LevelEntity();
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

                        entities.Add(itemData);
                    }

                    var camera = FindObjectOfType<CameraController>();
                    LevelData levelData = new LevelData
                    {
                        cameraInfo = new LevelManager.CameraInfo
                        {
                            fixedAtCentre = camera.isFixedAtCentre,
                            min = camera.min,
                            max = camera.max,
                        },
                        Content = entities.ToArray()
                    };

                    string json = EditorJsonUtility.ToJson(levelData, true);

                    string finalFile = "Assets/Resources/" + editor_LevelFile.name + ".json";

                    using (FileStream fs = new FileStream(finalFile, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }

                    UnityEditor.AssetDatabase.Refresh();
                }

                if (editor_doLoad)
                {
                    GameObject[] LevelObjects = GameObject.FindGameObjectsWithTag("LevelContent");

                    foreach(var item in LevelObjects)
                    {
                        DestroyImmediate(item);
                    }

                    //string finalFile = "Assets/Resources/" + fileName;

                    LevelData levelData = new LevelData();
                    TextAsset json = editor_LevelFile;
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

                    // Older levels dont have camera info, check that max is greater than zero because JSON wont return null where info is missing.
                    if (levelData.cameraInfo.max > 0.0f)
                    {
                        var cam = FindObjectOfType<CameraController>();
                        cam.isFixedAtCentre = levelData.cameraInfo.fixedAtCentre;
                        cam.min = levelData.cameraInfo.min;
                        cam.max = levelData.cameraInfo.max;
                    }
                }
                #endregion

                break;
            case 1: // Level Order
                // Field for the levels json file
                order_LevelsFile = (TextAsset)EditorGUILayout.ObjectField(order_LevelsFile, typeof(TextAsset), true);

                // Save and load button side by side
                EditorGUILayout.BeginHorizontal();
                    bool doSave = GUILayout.Button("Save");
                    bool doLoad = GUILayout.Button("Load");
                EditorGUILayout.EndHorizontal();

                // field to set the number of levels
                order_NumLevels = EditorGUILayout.IntField("Levels", order_NumLevels);

                // if the numLevels is greater than the length of the level array, make the level array bigger
                if (order_NumLevels != order_LevelFiles.Length)
                {
                    Array.Resize(ref order_LevelFiles, order_NumLevels);
                }

                // Minimum number of levels is 1
                if (order_NumLevels <= 0)
                    order_NumLevels = 1;

                // Put a scrollbar next to the level file fields
                order_ScrollPos = EditorGUILayout.BeginScrollView(order_ScrollPos);
                // add a field for each text asset (level file) up to the numLevels number
                for (int i = 0; i < order_NumLevels; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(20.0f));
                    order_LevelFiles[i] = (TextAsset)EditorGUILayout.ObjectField(order_LevelFiles[i], typeof(TextAsset), true);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();

                if (doSave)
                {
                    // Create a new LevelList object
                    LevelManager.LevelList levelList = new LevelManager.LevelList();
                    levelList.Levels = new string[order_NumLevels];
                    for (int i = 0; i < order_NumLevels; i++)
                    {
                        // fill the levelList with the names of the level files
                        levelList.Levels[i] = order_LevelFiles[i].name;
                    }


                    // Save it to json
                    string json = EditorJsonUtility.ToJson(levelList, true);

                    string finalFile = "Assets/Resources/" + order_LevelsFile.name + ".json";

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
                    // Make a new LevelList object
                    LevelManager.LevelList levelList = new LevelManager.LevelList();
                    // Load the json file into the LevelList
                    EditorJsonUtility.FromJsonOverwrite(order_LevelsFile.ToString(), levelList);

                    // Update the textAsset array with the contents of the file
                    order_NumLevels = levelList.Levels.Length;
                    order_LevelFiles = new TextAsset[order_NumLevels];
                    for (int i = 0; i < order_NumLevels; i++)
                    {
                        order_LevelFiles[i] = Resources.Load<TextAsset>(levelList.Levels[i]);
                    }
                }

                break;
            default:
                break;
        }
    }
}
