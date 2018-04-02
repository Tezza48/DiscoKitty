using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{



    [Header("Main Menu Panels")]
    public GameObject LandingScreen;
    public GameObject StartScreen;
    public GameObject LevelsScreen;

    private void Awake()
    {
        LandingScreen.SetActive(true);
        StartScreen.SetActive(false);
        LevelsScreen.SetActive(false);
    }

    public void OnClick_PlayMeow()
    {
        LandingScreen.SetActive(false);
        StartScreen.SetActive(true);
    }
    public void OnClick_Levels()
    {
        StartScreen.SetActive(false);
        LevelsScreen.SetActive(true);
    }
    public void OnClick_Quit()
    {
        Application.Quit();
    }
}
