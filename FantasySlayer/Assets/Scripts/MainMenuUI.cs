using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text gameName;
    private float tSize;
    private bool isIncreasing = true;

    private void Start()
    {
        tSize = 0.04f;
    }
    private void Update()
    {
        if (isIncreasing)
        {
            gameName.fontSize += tSize;

            if (gameName.fontSize >= 110)
            {
                isIncreasing = false;
            }
        }
        else
        {
            gameName.fontSize -= tSize;

            if (gameName.fontSize <= 100)
            {
                isIncreasing = true;
            }
        }
    }

    public void GameStart()
    {        
        SceneManager.LoadScene("Main");
    }

    public void GameExit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
