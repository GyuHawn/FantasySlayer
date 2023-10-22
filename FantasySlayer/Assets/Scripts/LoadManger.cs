using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manger : MonoBehaviour
{
    public GameObject manager;
    private void Start()
    {
        DontDestroyOnLoad(manager);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMeun")
        {
            Destroy(manager);
        }
    }
}
