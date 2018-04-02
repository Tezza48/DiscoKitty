﻿using System.Collections;
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

    private string fileName = "";

    private void OnGUI()
    {
        fileName = EditorGUILayout.TextField("Filename:", fileName);

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

            string finalFile = "Assets/Resources/" + fileName + ".json";

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
            TextAsset json = Resources.Load<TextAsset>(fileName);
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
    }
}
