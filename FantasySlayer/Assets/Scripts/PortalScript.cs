using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene("Stage1-1");
            }
            if (SceneManager.GetActiveScene().name == "Stage1-1")
            {
                SceneManager.LoadScene("Stage1-2");
            }
            if (SceneManager.GetActiveScene().name == "Stage1-2")
            {
                SceneManager.LoadScene("Stage1-3");
            }
        }
    }
}
