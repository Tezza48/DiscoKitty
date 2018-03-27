using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LevelList : MonoBehaviour
{
    public class LevelArrayData
    {
        public string[] Levels;
        public LevelArrayData()
        {
            Levels = new string[0];
        }
    }

    public GameObject LevelButtonPrefab;

    string[] levels;

    private void Start()
    {
        // Load level list from json file
        LevelArrayData levelArrayData = new LevelArrayData();
        TextAsset levelsJson = Resources.Load<TextAsset>("levels");
        JsonUtility.FromJsonOverwrite(levelsJson.ToString(), levelArrayData);
        levels = levelArrayData.Levels;

        int id = 1;
        foreach (var item in levels)
        {
            UI_LevelButton newButton = Instantiate(LevelButtonPrefab, transform).GetComponent<UI_LevelButton>();
            newButton.parentElement = this;
            newButton.level = item;
            newButton.transform.GetChild(0).GetComponent<Text>().text = id.ToString();
            id++;
        }
    }

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadGameScene(name));
    }

    IEnumerator LoadGameScene(string name)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

        yield return new WaitUntil(() => loading.isDone);
        Debug.Log("Level has finished loading");
        GameObject.Find("GameManager").GetComponent<GameManager>().SetLevel(name);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
