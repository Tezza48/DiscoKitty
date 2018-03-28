using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelButton : MonoBehaviour {

    public UI_LevelList parentElement;
    public int levelID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        LevelManager.Singleton.LoadLevel(levelID);
    }
}
