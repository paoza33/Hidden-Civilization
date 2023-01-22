using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceOpen : MonoBehaviour
{
    private bool playerAlreadyInteract;
    public Animator animator;
    public MeshCollider meshCollider;

    private void Awake()
    {
        enabled = false;
        animator.SetBool("Open", false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            playerAlreadyInteract = true;
            animator.SetBool("Open", true);
            meshCollider.sharedMesh = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
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
