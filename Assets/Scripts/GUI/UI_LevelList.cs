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
    public GameObject DummyButtonPrefab;


    private void Start()
    {
        GameSaveData data = GameData.LoadData();
        for(int id = 0; id < LevelManager.Singleton.levels.Length; id++)
        {
            if (id > data.highestLevel + 1)
            {
                Instantiate(DummyButtonPrefab, transform);
                continue;
            }
            string levelName = LevelManager.Singleton.levels[id];

            UI_LevelButton newButton = Instantiate(LevelButtonPrefab, transform).GetComponent<UI_LevelButton>();
            newButton.parentElement = this;

            float completionTime = 0.0f;

            // get player's progress data
            if (data.levelData.ContainsKey(levelName))
            {
                completionTime = data.levelData[levelName].completionTime;
            }

            // Initialize the button with player's data too
            bool isUnlocked = data.highestLevel > id;
            newButton.Init(id, completionTime, isUnlocked);
            //newButton.transform.GetChild(0).GetComponent<Text>().text = (id+1).ToString();
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
        //GameObject.Find("GameManager").GetComponent<GameManager>().SetLevel(name);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
