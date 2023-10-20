using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
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
