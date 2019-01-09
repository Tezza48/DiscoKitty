using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Editor_CustomScene : SceneView
{
	[MenuItem("Disco Kitty/Level Editor Scene")]
	public static void ShowWindow()
	{
		GetWindow(typeof(Editor_CustomScene));
	}

	public override void OnEnable()
	{
		titleContent = new GUIContent("Custom Scene");	
	}

	private void OnGUI()
	{

	}
}
