using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    public GameObject[] objects;
    private void Awake()
    {
        foreach (var obj in objects)
        {
            DontDestroyOnLoad(obj);
        }
    }
}