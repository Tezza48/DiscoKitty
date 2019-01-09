using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ObjectType = LevelEntity.ObjectType;

//[CustomEditor(typeof(LevelEntity)), CanEditMultipleObjects]
//public class Editor_LevelEntity : Editor
//{
//	SerializedProperty posAndRot;
//	SerializedProperty typeProperty;
//	SerializedProperty radiusProperty;

//	private void OnEnable()
//	{
//		posAndRot = serializedObject.FindProperty("positionAndRotation");
//		typeProperty = serializedObject.FindProperty("type");
//		radiusProperty = serializedObject.FindProperty("radius");
//	}

//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//		EditorGUILayout.BeginHorizontal();
//		{
//			Vector2 newPos = EditorGUILayout.Vector2Field("Position", (Vector2)posAndRot.vector3Value);
//			float newRot = EditorGUILayout.FloatField("Rotation", posAndRot.vector3Value.z);
//			EditorGUILayout.EndHorizontal();
//		}

//	}
//}
//[CustomPropertyDrawer(typeof(LevelEntity))]
public class LevelEntity_Drawer : PropertyDrawer
{
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{

		EditorGUI.BeginProperty(position, label, property);
		{
			var prop_type = property.FindPropertyRelative("type");
			ObjectType objectType = (ObjectType)prop_type.enumValueIndex;
			property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, new GUIContent(objectType.ToString()));
			if (property.isExpanded)
			{
				EditorGUILayout.PropertyField(prop_type, GUIContent.none);

				EditorGUILayout.PropertyField(property.FindPropertyRelative("position"));

				if (objectType != ObjectType.Zone)
					EditorGUILayout.PropertyField(property.FindPropertyRelative("rotation"));

				if (objectType == ObjectType.Zone)
					EditorGUILayout.PropertyField(property.FindPropertyRelative("radius"));
			}
		}

		EditorGUI.EndProperty();
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0f; }
}