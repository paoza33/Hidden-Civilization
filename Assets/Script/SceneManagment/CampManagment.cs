using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampManagment : MonoBehaviour
{
    public GameObject playerStart;
    private GameObject player;

    public Material[] youngSkin;

    public Transform spawnWoodenHut;
    public Transform spawnBeach;
    public Transform spawnWindow;
    private int state;
    public GameObject[] triggerState0, triggerState1, triggerState2, triggerState3;
    public Light skyLight;

    public Dialog dialogState0, dialogState1, dialogState2, dialogState3;
    public Dialog[] dialogsEN;

    private SaveDataSpawn spawn;

    public AudioClip audioClipDay, audioClipNight;
    private bool isNight;

    private bool isEnglish;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        state = SaveDataManager.LoadDataSceneState().campState;

        spawn = SaveDataManager.LoadDataSpawn();

        isEnglish = LocaleSelector.instance.IsEnglish();
        
        SettingsStart();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(Fade());
                enabled = false;
            }
        }
    }

    private void SettingsStart()
    {
        if(state == 0)  // enfant -> trigger beach, special tree, door ("C'est la cabane du vieil homme, il semble n'y avoir personne")
        {
            isNight = false;
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            Material[] tempMat = player.GetComponentInChildren<SkinnedMeshRenderer>().materials;

            for (int i = 0; i < tempMat.Length; i++)
            {
                tempMat[i].color = youngSkin[i].color;
            }

            foreach (GameObject obj in triggerState0)
            {
                obj.SetActive(true);
            }
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogsEN[0]);
            else
                DialogOpen.instance.StartDialog(dialogState0);
        }

        else if(state == 1) // enfant -> night, longfade "oops il fait d�j� nuit, door (joueur rentre dedans sans dialog), invisible wall village
        {
            isNight=true;
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Material[] tempMat = player.GetComponentInChildren<SkinnedMeshRenderer>().materials;

            for (int i = 0; i < tempMat.Length; i++)
            {
                tempMat[i].color = youngSkin[i].color;
            }

            playerStart.transform.position = spawnBeach.position;

            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogsEN[1]);
            else
                DialogOpen.instance.StartDialog(dialogState1);

            foreach (GameObject obj in triggerState1)
            {
                obj.SetActive(true);
            }

            skyLight.intensity = 0.2f;
        }

        else if (state == 2) // enfant -> night -> door ("J'entends des gens parler � l'interieur, je ne devrais pas rester l� longtemps")
        {
            isNight = true;
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            playerStart.transform.position = spawnWindow.position;

            foreach (GameObject obj in triggerState2)
            {
                obj.SetActive(true);
            }
            skyLight.intensity = 0.2f;
            
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogsEN[2]);
            else
                DialogOpen.instance.StartDialog(dialogState2);
        }

        else if(state == 3) // trigger boat -> direction lost island, door -> peut rentrer.
        {
            isNight = false;
            if (spawn.previousSceneName == "WoodenHut" && state == 3)
                playerStart.transform.position = spawnWoodenHut.position;

            foreach (GameObject obj in triggerState3)
            {
                obj.SetActive(true);
            }
            
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogsEN[3]);
            else
                DialogOpen.instance.StartDialog(dialogState3);
        }
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        if (isNight)
            AudioManager.instance.PlayThemeSong(audioClipNight);
        else
            AudioManager.instance.PlayThemeSong(audioClipDay);

        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}
