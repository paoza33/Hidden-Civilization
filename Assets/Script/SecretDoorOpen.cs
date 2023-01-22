using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretDoorOpen : MonoBehaviour
{
    private bool playerAlreadyInteract;
    public string keyNeeded;
    public Animator animatorDoor;
    public BoxCollider colliderToDesactivate;
    public Dialog open;
    public Dialog notOpen;
    private void Awake()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            playerAlreadyInteract = true;
            if (Inventory.instance.FindItem(keyNeeded))
            {
                animatorDoor.SetBool("PlayerHaveKey", true);
                colliderToDesactivate.enabled = false;
                DialogOpen.instance.StartDialog(open);
            }
            else
            {
                DialogOpen.instance.StartDialog(notOpen);
            }
        }
        else if(Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                playerAlreadyInteract = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
        }
    }
}
