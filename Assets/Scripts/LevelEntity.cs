using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelEntity
{
	public enum ObjectType
	{
		Cat,
		Pickle,
		Zone,
		Box,
		None
	}

	public ObjectType type;
	public Vector2 position;
	public float rotation;
	public float radius;
}