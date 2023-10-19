using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset;

    private bool checkPlayer = false;

    void Update()
    {
        if (player == null && !checkPlayer)
        {
            PlayerCheck();
            checkPlayer = true;
        }

        transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, transform.position.z);
    }

    void PlayerCheck()
    {
        player = GameObject.Find("Player");

    }
}