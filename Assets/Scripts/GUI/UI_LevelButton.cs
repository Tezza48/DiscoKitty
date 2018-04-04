using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelButton : MonoBehaviour {

    public UI_LevelList parentElement;
    public Text name;
    public Text time;
    private int levelID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(int id, float fastestTime, bool unlocked)
    {
        levelID = id;
        name.text = (id + 1).ToString();
        time.text = fastestTime > 0.0f ? fastestTime.ToString("N2") : "";
    }

    public void OnClick()
    {
        LevelManager.Singleton.LoadLevel(levelID);
    }
}
