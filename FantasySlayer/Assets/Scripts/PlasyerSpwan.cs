using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpwan : MonoBehaviour
{
    public GameObject mPlayer;
    public GameObject spawnPoint;

    private static bool isSpawn = false;

    void Start()
    {
        if (!isSpawn)
        {
            SpawnPlayer();
            isSpawn = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void SpawnPlayer()
    {
        GameObject newPlayer = Instantiate(mPlayer, spawnPoint.transform.position, Quaternion.identity);
        newPlayer.name = "Player";
        DontDestroyOnLoad(newPlayer);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage1-1")
        {
            GameObject player = GameObject.Find("Player");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
