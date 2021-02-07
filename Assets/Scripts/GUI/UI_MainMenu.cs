using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{



    [Header("Main Menu Panels")]
    public GameObject LandingScreen;
    public GameObject StartScreen;
    public GameObject LevelsScreen;
    public GameObject SettingsScreen;

    private void Awake()
    {
        LandingScreen.SetActive (true);
        StartScreen.SetActive   (false);
        LevelsScreen.SetActive(false);
        SettingsScreen.SetActive(false);
    }

    public void OnClick_StartScreen()
    {
        StartScreen.SetActive   (true);
        LandingScreen.SetActive (false);
        LevelsScreen.SetActive(false);
        SettingsScreen.SetActive(false);
    }
    public void OnClick_Levels()
    {
        LevelsScreen.SetActive(true);
        StartScreen.SetActive(false);
        LandingScreen.SetActive(false);
        SettingsScreen.SetActive(false);
    }
    public void OnClick_Settings()
    {
        LevelsScreen.SetActive(false);
        StartScreen.SetActive(false);
        LandingScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }
    public void OnClick_Quit()
    {
        Application.Quit();
    }
}
