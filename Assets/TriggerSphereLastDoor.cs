using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSphereLastDoor : MonoBehaviour
{
    public GameObject sphere, road;
    private bool alreadyInteract = false;
    public int id;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !alreadyInteract)
        {
            alreadyInteract = true;
            LastDoorManagment.instance.AddOrderPlayer(id);
            sphere.GetComponent<FlickeringEmissive>().enabled = false;
            road.GetComponent<FlickeringEmissive>().enabled = true;
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sphere.GetComponent<FlickeringEmissive>().isReverse = false;
            sphere.GetComponent<FlickeringEmissive>().enabled = true;
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alreadyInteract = false;
            sphere.GetComponent<FlickeringEmissive>().isReverse = true;
            enabled = false;
        }
    }
}
