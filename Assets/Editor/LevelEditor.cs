using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using LevelDataArray = LevelManager.LevelDataArray;
using ObjectType = LevelEntity.ObjectType;

public class LevelEditor : EditorWindow
{
	public GUIStyle style;
	private Level currentLevel;

	public Texture EntityTexture;

	[MenuItem("Disco Kitty/Level Editor")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
  
		GetWindow(typeof(LevelEditor));

	}

	private void OnEnable()
	{
		EntityTexture = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Art/app-icon.png", typeof(Texture));
	}

	private void OnGUI()
	{
		currentLevel = (Level)EditorGUILayout.ObjectField(currentLevel, typeof(Level), true);
		foreach (LevelEntity entity in currentLevel.entities)
		{
			DrawEntity(entity);
		}
	}

	private void DrawEntity(LevelEntity entity)
	{
		EditorGUI.DrawTextureTransparent(new Rect(Vector2.zero, position.size), null, ScaleMode.ScaleToFit, position.width/position.height);
		EditorGUI.DrawTextureTransparent(new Rect(position.size/2.0f - new Vector2(50, 50) + entity.position * 100.0f, Vector2.one *  100.0f), EntityTexture);
	}
}
