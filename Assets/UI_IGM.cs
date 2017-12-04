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

    // Use this for initialization
    void Start()
    {

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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
