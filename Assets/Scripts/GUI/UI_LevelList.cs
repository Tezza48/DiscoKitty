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



    private void Start()
    {
        for(int id = 0; id < LevelManager.Singleton.levels.Length; id++)
        {
            UI_LevelButton newButton = Instantiate(LevelButtonPrefab, transform).GetComponent<UI_LevelButton>();
            newButton.parentElement = this;
            newButton.levelID = id;
            newButton.transform.GetChild(0).GetComponent<Text>().text = (id+1).ToString();
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
