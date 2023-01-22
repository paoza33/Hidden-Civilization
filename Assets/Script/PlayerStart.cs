using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
        CameraMovement.instance.StartPosition(PlayerMovement.instance.GetTransform());
    }
}
