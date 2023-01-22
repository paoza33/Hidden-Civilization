using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    private bool playerAlreadyInteract;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            playerAlreadyInteract = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            enabled = false;
        }
    }
}