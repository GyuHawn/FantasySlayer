using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mgameOver;

    public static Action gameOver;

    private void Awake()
    {
        gameOver = () => { GameOver(); };
    }

    private void Start()
    {
        mgameOver.SetActive(false);
    }

    public void GameOver()
    {
        mgameOver.SetActive(true);
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Main");
    }
}
