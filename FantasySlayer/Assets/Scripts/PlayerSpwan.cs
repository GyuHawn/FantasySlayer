using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpwan : MonoBehaviour
{
    public GameObject mPlayer;
    public GameObject spawnPoint;

    private static bool isSpawn = false;
    private GameObject newPlayer;

    void Start()
    {
        if (!isSpawn)
        {
            SpawnPlayer();
            isSpawn = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMeun")
        {
            Destroy(newPlayer);
            isSpawn = false;
        }
    }

    void SpawnPlayer()
    {
        newPlayer = Instantiate(mPlayer, spawnPoint.transform.position, Quaternion.identity);
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
