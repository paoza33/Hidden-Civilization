using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManagment : MonoBehaviour
{
    private bool ifLongFade;
    public Dialog dialogState0;
    public Dialog dialogState1;
    public Dialog dialogState2;
    public Dialog dialogState3;
    public Dialog dialogState4;
    public Dialog dialogState5;

    private Animator animator;

    public Transform spawnBedroom;
    private GameObject player;

    public bool unfixX;
    public bool unfixZ;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        SaveDataSceneState saveDataSceneState = SaveDataManager.LoadDataSceneState();

        if(saveDataSceneState == null)
        {
            Debug.LogError("empty save file data scene state");
            return;
        }

        else if(saveDataSceneState != null && saveDataSceneState.homeState == 0)    // présent, joueur doit prendre livre
        {
            player.transform.position = spawnBedroom.position;
            ifLongFade = true;
            saveDataSceneState.homeState = 1;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
            DialogOpen.instance.StartDialog(dialogState0);
        }

        else if(saveDataSceneState != null && saveDataSceneState.homeState == 1)    //passé, flashback
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.homeState = 2;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
            DialogOpen.instance.StartDialog(dialogState1);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 2)   // retour présent, joueur explique où il doit aller
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.homeState = 3;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
            DialogOpen.instance.StartDialog(dialogState2);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 3)   // retour library, il explique qu'i reconnait le signe et qu'il doit aller vers forêt
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.homeState = 4;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
            DialogOpen.instance.StartDialog(dialogState2);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 4)   // retour wood, il se demande qui était cet homme et qu'il doit partir maintenant qu'il fait nuit
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.homeState = 5;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
            DialogOpen.instance.StartDialog(dialogState2);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 5)   // retour Cave, "Je me suis assez reposé, il est temps d'aller au ruins"
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.homeState = 6;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
            DialogOpen.instance.StartDialog(dialogState2);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 6)   // default
        {
            ifLongFade= false;
        }

        if (!ifLongFade)
        {
            StartCoroutine(StartingFade());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(StartingFade());
            }
        }
    }

    private IEnumerator StartingFade()
    {
        PlayerMovement.instance.StopMovement();
        enabled = false;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        if (unfixX)
            CameraMovement.instance.cameraFixX = false;
        if (unfixZ)
            CameraMovement.instance.cameraFixZ = false;
    }
}
