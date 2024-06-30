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
    public MeshCollider meshColliderDesactivate;
    public BoxCollider colliderToActivate;
    public BoxCollider colliderDoorBathroom;    // on rajoute plusieurs variables s'utilisant que dans un cas, car flemme de refaire un script pour chacun de ces cas 

    public bool ifChangeStateCamp; // pareil

    public Dialog open, openEN;
    public Dialog notOpen, notOpenEN;

    public bool isNeedKey;

    public bool isItemObtained;
    public Item item;
    public bool destroyObject;
    public bool selfDesactivation;

    public bool isChangingScene;
    public string levelToLoad;

    public GameObject pnjAppear;  // seulement pour le cas de la scene WoodenHut

    private Animator animator;

    private TextMeshProUGUI textInteract;

    public bool ifLibrary;
    private bool ifAlreadyLibraryInteraction; // on incrémente de 1 l'etat de library en state 0

    public AudioClip audioTeleport;

    private bool isEnglish;

    private void Awake()
    {
        isEnglish = LocaleSelector.instance.IsEnglish();
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
                    if(colliderToDesactivate != null){
                        colliderToDesactivate.enabled = false;
                        textInteract.enabled = false;
                    }
                    if(isEnglish)
                        DialogOpen.instance.StartDialog(openEN);
                    else
                        DialogOpen.instance.StartDialog(open);
                }
                else
                {
                    if(isEnglish)
                        DialogOpen.instance.StartDialog(openEN);
                    else
                        DialogOpen.instance.StartDialog(open);
                }
            }
            else
            {
                if(isEnglish)
                    DialogOpen.instance.StartDialog(openEN);
                else
                    DialogOpen.instance.StartDialog(open);
            }
        }
        else if(Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                if(ifLibrary && !ifAlreadyLibraryInteraction){// cas de library en state0
                    ifAlreadyLibraryInteraction = true;
                    LibraryManagment.instance.SetupState0();
                }

                playerAlreadyInteract = false;
                if (ifChangeStateCamp)
                {
                    SaveDataSceneState data = SaveDataManager.LoadDataSceneState();
                    data.woodenHutState += 1;   // here
                    data.campState += 1;    // here
                    SaveDataManager.SaveDataSceneState(data);
                }
                if(pnjAppear != null)
                    pnjAppear.SetActive(true);

                if (colliderToDesactivate != null)
                {
                    colliderToDesactivate.enabled = false;
                    textInteract.enabled = false;
                    enabled = false;
                }
                if(meshColliderDesactivate != null){
                    meshColliderDesactivate.enabled = false;
                    enabled = false;
                }
                if(colliderToActivate != null)
                {
                    colliderToActivate.enabled = true;
                }
                if(colliderDoorBathroom != null)
                {
                    colliderDoorBathroom.enabled = true;
                }
                if(selfDesactivation)
                    GetComponent<BoxCollider>().enabled = false;
                if (isChangingScene && !isNeedKey)
                {
                    enabled = false;

                    SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
                    data.currentSceneName = levelToLoad;
                    data.previousSceneName = SceneManager.GetActiveScene().name;

                    SaveDataManager.SaveDataSpawn(data);

                    PlayerMovement.instance.StopMovement();
                    StartCoroutine(Fade());
                }
                else if (isChangingScene && isNeedKey)
                {
                    if (Inventory.instance.FindItem(keyNeeded))
                    {
                        enabled = false;

                        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
                        data.currentSceneName = levelToLoad;
                        data.previousSceneName = SceneManager.GetActiveScene().name;

                        SaveDataManager.SaveDataSpawn(data);

                        PlayerMovement.instance.StopMovement();
                        StartCoroutine(Fade());
                    }
                }
                else
                {
                    if(colliderToDesactivate == null)
                        textInteract.enabled = true;
                }
                if (isItemObtained)
                {
                    enabled = false;
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
        AudioManager.instance.StopCurrentSong();
        if(SceneManager.GetActiveScene().name == "Tower")
            AudioManager.instance.PlayClipAt(audioTeleport, transform.position);

        CameraMovement.instance.enabled = true;
        SceneManager.LoadScene(levelToLoad);
    }
}
