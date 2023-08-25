using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTrigger : MonoBehaviour
{
    public BoxCollider[] triggerToActivate;
    public bool playerNeedInteraction;

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            for (int i = 0; i < triggerToActivate.Length; i++)
            {
                triggerToActivate[i].enabled = true;
            }
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerNeedInteraction)
            {
                enabled = true;
            }
            else
            {
                for (int i = 0; i < triggerToActivate.Length; i++)
                {
                    triggerToActivate[i].enabled = true;
                }
            }
        }
    }
}
