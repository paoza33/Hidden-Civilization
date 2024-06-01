using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Dialog notOpen;
    private TextMeshProUGUI text;

    public bool ifLibraryDoorState0;
    private bool libraryDoorAlreadyInteract;

    public AudioClip audioClip;

    private bool sceneChanging = false; // true lorsque la scene est en train d'etre modifie
    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        text = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        enabled = false;
    }
    private void Update()
    {
        if (!sceneChanging)
        {
            if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
            {
                text.enabled = false;
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
                    if(ifLibraryDoorState0 && !libraryDoorAlreadyInteract){
                        libraryDoorAlreadyInteract = true;
                        LibraryManagment.instance.SetupState0();
                    }
                    
                    if (ifChangeLevel)
                    {
                        PlayerMovement.instance.StopMovement();
                        StartCoroutine(Fade());
                    }
                    else
                    {
                        text.enabled = true;
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
        text.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
        }
        text.enabled = false;
    }

    public IEnumerator Fade()
    {
        AudioManager.instance.PlayClipAt(audioClip, transform.position);
        sceneChanging = true;
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.75f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        data.currentSceneName = levelToLoad;
        data.previousSceneName = SceneManager.GetActiveScene().name;

        SaveDataManager.SaveDataSpawn(data);

        SceneManager.LoadScene(levelToLoad);
    }
}
