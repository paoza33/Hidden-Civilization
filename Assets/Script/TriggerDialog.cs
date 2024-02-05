using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TriggerDialog : MonoBehaviour
{
    private bool playerAlreadyInteract;

    public string keyNeeded;

    public Animator animatorDoor;

    public BoxCollider colliderToDesactivate;

    public Dialog open;
    public Dialog notOpen;

    public bool isNeedKey;

    public bool isChangingScene;
    public string levelToLoad;

    private Animator animator;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
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
            if (isNeedKey)  // si on a besoin d'une cl� pour ouvrir le dialogue
            {
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
            else
            {
                DialogOpen.instance.StartDialog(open);
            }
        }
        else if(Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                playerAlreadyInteract = false;
                if (colliderToDesactivate != null)
                {
                    colliderToDesactivate.enabled = false;
                }
                if(isChangingScene){
                    PlayerMovement.instance.StopMovement();
                    StartCoroutine(Fade());
                }
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

    private IEnumerator Fade()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.75f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
