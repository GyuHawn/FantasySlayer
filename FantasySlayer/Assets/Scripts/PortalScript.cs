using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    public GameObject portalUI;
    public bool userPortal;

    public bool useUI;

    private void Start()
    {
        useUI = false;
        portalUI.SetActive(false);
        userPortal = false;

        // Depending on the current scene, start the appropriate coroutine
        if (SceneManager.GetActiveScene().name == "Main")
            StartCoroutine(EnableUiAfterTime(5));  // 5 seconds for Main scene
        else if (SceneManager.GetActiveScene().name.StartsWith("Stage1-"))
            StartCoroutine(EnableUiAfterTime(30)); // 30 seconds for Stage1-x scenes
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (useUI)
        {
            portalUI.SetActive(true);

            if (userPortal)
            {
                if (SceneManager.GetActiveScene().name == "Main")
                {
                    SceneManager.LoadScene("Stage1-3");
                }
                else if (SceneManager.GetActiveScene().name == "Stage1-1")
                {
                    SceneManager.LoadScene("Stage1-2");
                }
                else if (SceneManager.GetActiveScene().name == "Stage1-2")
                {
                    SceneManager.LoadScene("Stage1-3");
                }
                else if (SceneManager.GetActiveScene().name == "Stage1-3")
                {
                    SceneManager.LoadScene("MainMeun");

                    StartCoroutine(EnableUiAfterTime(5));  // 5 seconds for Main scene again
                }

                portalUI.SetActive(false);
                useUI = false;
                userPortal = false;
            }
        }
    }

    IEnumerator EnableUiAfterTime(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        useUI = true;
    }

    public void UsePortal()
    {
        userPortal = true;
    }
}
