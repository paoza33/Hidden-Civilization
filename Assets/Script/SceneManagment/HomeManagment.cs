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

    public Dialog[] dialogsEN;

    public Material[] recentSkin, youngSkin;

    private Animator animator;

    public Transform spawnBedroom;
    private GameObject player;

    public bool unfixX;
    public bool unfixZ;

    public GameObject[] objState0, objState1;
    public GameObject laptop;

    public AudioClip audioClip;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        SaveDataSceneState saveDataSceneState = SaveDataManager.LoadDataSceneState();

        if(saveDataSceneState == null)
        {
            Debug.LogError("empty save file data scene state");
            return;
        }

        else if(saveDataSceneState != null && saveDataSceneState.homeState == 0)    // pr�sent, joueur doit prendre livre
        {
            foreach(GameObject obj in objState0)
                obj.SetActive(true);

            player.transform.position = spawnBedroom.position;
            ifLongFade = true;
            DialogOpen.instance.StartDialog(dialogState0);
        }

        else if(saveDataSceneState != null && saveDataSceneState.homeState == 1)    //pass�, flashback
        {
            foreach (GameObject obj in objState1)
                obj.SetActive(true);

            Material[] tempMat = player.GetComponentInChildren<SkinnedMeshRenderer>().materials;

            for(int i = 0; i < tempMat.Length; i++)
            {
                tempMat[i].color = youngSkin[i].color;
            }
            laptop.SetActive(false);
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            DialogOpen.instance.StartDialog(dialogState1);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 2)   // retour pr�sent, joueur explique o� il doit aller
        {
            Material[] tempMat = player.GetComponentInChildren<SkinnedMeshRenderer>().materials;

            for (int i = 0; i < tempMat.Length; i++)
            {
                tempMat[i].color = recentSkin[i].color;
            }
            player.transform.localScale = new Vector3(1f, 1f, 1f); // retour taille normal
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            DialogOpen.instance.StartDialog(dialogState2);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 3)   // retour library, il explique qu'i reconnait le signe et qu'il doit aller vers for�t
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.villageState = 3;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);

            DialogOpen.instance.StartDialog(dialogState3);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 4)   // retour wood, il se demande qui �tait cet homme et qu'il doit partir maintenant qu'il fait nuit
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            DialogOpen.instance.StartDialog(dialogState4);
            saveDataSceneState.woodState =1;
            saveDataSceneState.villageState = 5;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 5)   // retour Cave, "Je me suis assez repos�, il est temps d'aller au ruins"
        {
            ifLongFade = true;
            player.transform.position = spawnBedroom.position;
            saveDataSceneState.cityState = 2;   // here
            saveDataSceneState.villageState = 6;    // here
            SaveDataManager.SaveDataSceneState(saveDataSceneState);

            DialogOpen.instance.StartDialog(dialogState5);
        }

        else if (saveDataSceneState != null && saveDataSceneState.homeState == 6)   // default
        {
            enabled = false;
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
        AudioManager.instance.PlayThemeSong(audioClip);
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        if (unfixX)
            CameraMovement.instance.cameraFixX = false;
        if (unfixZ)
            CameraMovement.instance.cameraFixZ = false;
    }
}
