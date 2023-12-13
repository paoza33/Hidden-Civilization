using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private bool alreadyInteract = false;
    public Dialog dialog;
    private void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && !alreadyInteract)
        {
            alreadyInteract = true;
            DialogOpen.instance.StartDialog(dialog);
        }
        else if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                // ajouter l'item
                alreadyInteract = false;
                PlayerMovement.instance.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
        }
    }
}