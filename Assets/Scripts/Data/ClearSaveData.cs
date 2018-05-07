using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSaveData : MonoBehaviour
{

    public void OnClearData()
    {
        PlayerPrefs.DeleteAll();
        GameData.ClearData();
    }
}
