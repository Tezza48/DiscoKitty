using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class LevelList : ScriptableObject
{
    public string[] levels;

#if UNITY_EDITOR
    public SceneAsset[] Scenes
    {
        get {
            return levels.Select(level => AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/" + level + ".unity")).ToArray();
        }
        set {
            levels = value.Select(scene => AssetDatabase
                .GetAssetPath(scene)
                .TrimStart("Assets/".ToCharArray())
                .TrimEnd(".unity".ToCharArray())
            ).ToArray();
        }
    }
#endif
}
