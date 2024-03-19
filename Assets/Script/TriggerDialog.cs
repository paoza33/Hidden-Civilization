using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class TriggerDialog : MonoBehaviour
{
    private bool playerAlreadyInteract;

    public string keyNeeded;

    public Animator animatorDoor;

    public BoxCollider colliderToDesactivate;
    public BoxCollider colliderToActivate;
    public BoxCollider colliderDoorBathroom;    // on rajoute plusieurs variables s'utilisant que dans un cas, car flemme de refaire un script pour chacun de ces cas 

    public bool ifChangeStateCamp; // pareil

    public Dialog open;
    public Dialog notOpen;

    public bool isNeedKey;

    public bool isItemObtained;
    public Item item;
    public bool destroyObject;

    public bool isChangingScene;
    public string levelToLoad;

    public GameObject pnjAppear;  // seulement pour le cas de la scene WoodenHut

    private Animator animator;

    private TextMeshProUGUI textInteract;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        enabled = false;
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
            textInteract.enabled = true;
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            playerAlreadyInteract = true;
            textInteract.enabled = false;
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
                if (ifChangeStateCamp)
                {
                    SaveDataSceneState data = SaveDataManager.LoadDataSceneState();
                    data.woodenHutState += 1;
                    data.campState += 1;
                    SaveDataManager.SaveDataSceneState(data);
                }
                if(pnjAppear != null)
                    pnjAppear.SetActive(true);

                if (colliderToDesactivate != null)
                {
                    colliderToDesactivate.enabled = false;
                }
                if(colliderToActivate != null)
                {
                    colliderToActivate.enabled = true;
                }
                if(colliderDoorBathroom != null)
                {
                    colliderDoorBathroom.enabled = true;
                }
                if (isChangingScene && !isNeedKey)
                {
                    PlayerMovement.instance.StopMovement();
                    StartCoroutine(Fade());
                }
                else if (isChangingScene && isNeedKey)
                {
                    if (Inventory.instance.FindItem(keyNeeded))
                    {
                        PlayerMovement.instance.StopMovement();
                        StartCoroutine(Fade());
                    }
                }
                else
                {
                    textInteract.enabled = true;
                }
                if (isItemObtained)
                {
                    Inventory.instance.AddItem(item);
                    textInteract.enabled = false;
                    if (destroyObject)
                    {
                        transform.parent.gameObject.SetActive(false);
                    }
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
        textInteract.enabled = false;
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
