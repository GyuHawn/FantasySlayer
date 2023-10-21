using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManger : MonoBehaviour
{
    public GameObject manager;
    private void Start()
    {
        DontDestroyOnLoad(manager);
    }
}
