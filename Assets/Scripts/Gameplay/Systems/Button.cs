using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
	private float isDown;
	public Door door;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log(collider.name);
		door.Open();
	}

	public void OnTriggerExit2D(Collider2D collider) {
		door.Close();
	}
}
