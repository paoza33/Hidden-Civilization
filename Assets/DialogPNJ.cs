using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogPNJ : MonoBehaviour
{
    private bool playerAlreadyInteract = false;

    public Dialog dialog;
    public bool needInteraction;
    public Item item;

    private TextMeshProUGUI textInteract;

    private void Awake()
    {
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        enabled = false;
    }

    void Update()
    {
        if (needInteraction)
        {
            if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
            {
                textInteract.enabled = false;
                playerAlreadyInteract = true;
                DialogOpen.instance.StartDialog(dialog);
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    if(item != null)
                        Inventory.instance.AddItem(item);
                    playerAlreadyInteract = false;
                    textInteract.enabled = true;
                }
            }
        }
        else
        {
            if (!playerAlreadyInteract)
            {
                DialogOpen.instance.StartDialog(dialog);
                playerAlreadyInteract = true;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    playerAlreadyInteract = false;
                    needInteraction = true;
                }
            }   
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (needInteraction)
                textInteract.enabled = true;
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (needInteraction)
                textInteract.enabled = false;
            enabled = false;
        }
    }
}
