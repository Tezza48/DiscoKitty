using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelButton : MonoBehaviour {

    public UI_LevelList parentElement;
    new public Text name;
    public Text time;
    public MedalDisplay medalDisplay;
    private int levelID;


    private float targetTime1 = 20.0f;
    private float targetTime2 = 10.0f;
    private float targetTime3 = 5.0f;

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


        int numMedals = 0;
        if (fastestTime < targetTime1)
        {
            numMedals = 1;
        }
        if (fastestTime < targetTime2)
        {
            numMedals = 2;
        }
        if (fastestTime < targetTime3)
        {
            numMedals = 3;
        }
        medalDisplay.ShowMedals(numMedals);
    }

    public void OnClick()
    {
        LevelManager.Singleton.LoadLevel(levelID);
    }
}
