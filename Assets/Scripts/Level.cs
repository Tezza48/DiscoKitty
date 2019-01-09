using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Disco Kitty/Level", order = 0)]
public class Level : ScriptableObject
{
	public enum LevelTheme
	{
		Desert
	}
	public LevelTheme theme;
	public LevelEntity[] entities;
}
