using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	private bool isOpen = false;
	public Transform doorHolder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isOpen) {
			doorHolder.localScale = Vector2.Lerp(doorHolder.localScale, new Vector2(0.1f, 1.0f), Time.deltaTime);
		} else {
			doorHolder.localScale = Vector2.Lerp(doorHolder.localScale, new Vector2(1.0f, 1.0f), Time.deltaTime);

		}
	}

	public void Open() {
		isOpen = true;
	}

	public void Close() {
		isOpen = false;
	}
}
