using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Policer : MonoBehaviour
{
    public Animator animatorPlayer;
    public Animator animatorDialog;
    private bool playerAlreadyInteract = false;
    public AudioClip clip;

    private void Awake()
    {
        enabled = false;
    }
    private void Update()
    {

        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            animatorDialog.SetBool("isOpen", true);
            playerAlreadyInteract = true;
            animatorPlayer.SetBool("AttackIdle", true);
            AudioManager.instance.PlayClipAt(clip, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            enabled = false;
            animatorDialog.SetBool("isOpen", false);
            playerAlreadyInteract = false;
            animatorPlayer.SetBool("AttackIdle", false);
        }
    }
}
