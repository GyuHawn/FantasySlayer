using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMove : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
       
    void Update()
    {
        transform.position += new Vector3(0.005f, 0, 0);

        if(transform.position.x >= endPos.x)
        {
            transform.position = startPos;
        }
    }
}
