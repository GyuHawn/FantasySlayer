using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manger : MonoBehaviour
{
    public GameObject manager;
    private void Start()
    {
        DontDestroyOnLoad(manager);
    }
}
