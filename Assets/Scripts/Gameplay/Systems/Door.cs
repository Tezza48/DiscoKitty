using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	private int inputs = 0;
	public Transform doorHolder;

	private float openProgress = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		openProgress += inputs > 0 ? Time.deltaTime : - Time.deltaTime;
		openProgress = Mathf.Clamp01(openProgress);
		
		doorHolder.localScale = Vector2.Lerp(new Vector2(1.0f, 1.0f), new Vector2(0.1f, 1.0f), openProgress);
	}

	public void Open() {
		inputs++;
	}

	public void Close() {
		inputs--;
	}
}
