using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_IGM : MonoBehaviour
{

    bool isOpen = false;
    public RectTransform mMenu;
    public float openSpeed = 1.0f;

    public Vector2 open, closed;

    public GameObject settingsDialog;

    // Use this for initialization
    void Start()
    {
        settingsDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mMenu.pivot = Vector2.Lerp(mMenu.pivot, isOpen ? open : closed, Time.deltaTime * openSpeed);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
    }

    public void ResetLevel ()
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.currentLevelID);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevelListMenu());
    }

    public void OnClick_Settings()
    {
        settingsDialog.SetActive(true);
        isOpen = false;
    }

    public void OnClick_SettingsClose()
    {
        settingsDialog.SetActive(false);
    }

    IEnumerator LoadLevelListMenu()
    {
        // save current scene
        Scene currentScene = SceneManager.GetActiveScene();
        // scene we sill be loading
        Scene menuScene = SceneManager.GetSceneByBuildIndex(0);

        // load the new scene
        AsyncOperation loading = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loading.isDone);

        // when the new scene has finished loading, open the levels menu
        FindObjectOfType<UI_MainMenu>().OnClick_Levels();

        // unload the original scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
