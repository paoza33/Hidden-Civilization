using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerSphereLastDoor : MonoBehaviour
{
    public GameObject sphere;
    private bool alreadyInteract = false;
    public int id;
    private TextMeshProUGUI textInteract;

    private void Awake()
    {
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !alreadyInteract)
        {
            alreadyInteract = true;
            LastDoorManagment.instance.AddOrderPlayer(id);
            sphere.GetComponent<FlickeringEmissive>().isReverse = false;
            sphere.GetComponent<FlickeringEmissive>().enabled = true;
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInteract.enabled = true;
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
            textInteract.enabled = false;
        }
    }

    public void ResetInteraction()
    {
        StartCoroutine(DelayResetInteraction());
    }

    private IEnumerator DelayResetInteraction()
    {
        yield return new WaitForSeconds(0.3f);
        alreadyInteract = false;
        sphere.GetComponent<FlickeringEmissive>().isReverse = true;
    }
}
