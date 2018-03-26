using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string firstLevelName = "Level_C1Circle1";

    public void BtnPlayPressed()
    {
        SceneManager.LoadScene(firstLevelName);
    }
    
}
