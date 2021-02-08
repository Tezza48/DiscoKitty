using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    SceneAsset[] scenes;
    SerializedProperty levelsProperty;

    bool scenesFoldOut = true;
    bool scenesListFoldOut = true;

    private void OnEnable()
    {
        levelsProperty = serializedObject.FindProperty("levels");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((LevelManager)target), typeof(LevelManager), false);

        scenesFoldOut = EditorGUILayout.BeginFoldoutHeaderGroup(scenesFoldOut, "Levels");
        EditorGUI.indentLevel++;
        var numScenes = EditorGUILayout.IntField("Size", levelsProperty.arraySize);
        levelsProperty.arraySize = numScenes;
        Array.Resize(ref scenes, numScenes);

        for (int i = 0; i < levelsProperty.arraySize; i++)
        {
            scenes[i] = (SceneAsset)EditorGUILayout.ObjectField("Element " + i, scenes[i], typeof(SceneAsset), true);
            levelsProperty.GetArrayElementAtIndex(i).stringValue = AssetDatabase.GetAssetPath(scenes[i]);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Debug Levels list");
        EditorGUILayout.PropertyField(levelsProperty);


        serializedObject.ApplyModifiedProperties();
    }
}
