using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TakeDoor : MonoBehaviour
{
    private bool playerAlreadyInteract;
    private bool ifChangeLevel = false;
    public string levelToLoad;
    private Animator animator;
    public bool isKeyNeeded;
    public string keyName;
    public Dialog open;
    private bool test;
    public Dialog notOpen;

    private bool sceneChanging = false; // true lorsque la scene est en train d'etre modifie
    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        enabled = false;
    }
    private void Update()
    {
        if (!sceneChanging)
        {
            if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
            {
                playerAlreadyInteract = true;
                if (isKeyNeeded)
                {
                    if (Inventory.instance.FindItem(keyName))
                    {
                        ifChangeLevel = true;
                        DialogOpen.instance.StartDialog(open);
                    }
                    else
                    {
                        ifChangeLevel = false;
                        DialogOpen.instance.StartDialog(notOpen);
                    }
                }
                else
                {
                    PlayerMovement.instance.StopMovement();
                    StartCoroutine(Fade());
                }
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    playerAlreadyInteract = false;
                    if (ifChangeLevel)
                    {
                        PlayerMovement.instance.StopMovement();
                        StartCoroutine(Fade());
                    }
                }
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
        }
    }

    public IEnumerator Fade()
    {
        sceneChanging = true;
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.50f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
