using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset; // Keep this as Vector2

    void Update()
    {
        // Create a new Vector3 using x and y from offset and z from current camera position.
        transform.position = new Vector3(player.transform.position.x + offset.x,
                                         player.transform.position.y + offset.y,
                                         transform.position.z);
    }
}