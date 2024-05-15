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

    public GameObject objToDesactivate;
    public GameObject objToActivate;

    public bool isRuinsFlash;
    public Dialog beforeFlash;
    public Dialog afterFlash;

    public bool ifFlashFinishing;

    public GameObject bodyguardLeft;
    public GameObject bodyguardRight;

    private Animator animator;

    private void Awake()
    {
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        animator = GameObject.FindGameObjectWithTag("Flash").GetComponent<Animator>();
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
                if(!isRuinsFlash)
                {
                    DialogOpen.instance.StartDialog(dialog);
                    playerAlreadyInteract = true;
                }
                else
                {
                    bodyguardLeft.transform.rotation = Quaternion.Euler(bodyguardLeft.transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
                    bodyguardRight.transform.rotation = Quaternion.Euler(bodyguardRight.transform.rotation.eulerAngles + new Vector3(0, 90f, 0));

                    DialogOpen.instance.StartDialog(beforeFlash);
                    playerAlreadyInteract = true;
                }               
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    if ((!isRuinsFlash))
                    {
                        playerAlreadyInteract = false;
                        needInteraction = true;
                    }
                    else
                    {
                        if(!ifFlashFinishing){
                            StartCoroutine(Flash());
                        }
                        else{
                            objToActivate.SetActive(true);
                            objToDesactivate.SetActive(false);
                        }
                    }
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

    private IEnumerator Flash(){
        enabled = false;
        PlayerMovement.instance.StopMovement();
        animator.SetTrigger("FlashIn");
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("FlashOut");
        ifFlashFinishing = true;
        DialogOpen.instance.StartDialog(afterFlash);
        enabled = true;
    }
}