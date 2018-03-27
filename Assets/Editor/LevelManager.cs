using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelManager : EditorWindow
{
    public enum ObjectType
    {
        Cat,
        Pickle,
        Zone,
        None
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

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LevelManager));
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
                if (zone)
                {
                    itemData.Type = ObjectType.Zone;
                    itemData.Radius = zone.size;
                }
                else if (cat)
                {
                    itemData.Type = ObjectType.Cat;
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
                }
            }
        }
    }
}
