using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TakeDoor : MonoBehaviour
{
    private bool playerAlreadyInteract;
    public string levelToLoad;
    private Animator animator;
    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        enabled = false;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            playerAlreadyInteract = true;
            PlayerMovement.instance.StopMovement();
            StartCoroutine(Fade());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
        }
    }

    public IEnumerator Fade()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.50f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
