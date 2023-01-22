using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Limit_X : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraMovement.instance.cameraFixX = true;
            CameraMovement.instance.fixX = player.transform.position.x;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraMovement.instance.cameraFixX = false;
        }
    }
}
