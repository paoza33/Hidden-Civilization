using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampManagment : MonoBehaviour
{
    public GameObject playerStart;
    private GameObject player;

    public Transform spawnWoodenHut;
    public Transform spawnBeach;
    public Transform spawnWindow;
    private int state;
    public GameObject[] triggerState0, triggerState1, triggerState2, triggerState3;
    public Light skyLight;

    public Dialog dialogState1;

    private SaveDataSpawn spawn;

    private void Awake()
    {
        enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");

        state = SaveDataManager.LoadDataSceneState().campState;

        spawn = SaveDataManager.LoadDataSpawn();


        SettingsStart();
    }

    private void SettingsStart()
    {
        if(state == 0)  // enfant -> trigger beach, special tree, door ("C'est la cabane du vieil homme, il semble n'y avoir personne")
        {
            Debug.Log("zero");
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            foreach (GameObject obj in triggerState0)
            {
                obj.SetActive(true);
            }
            StartCoroutine(Fade());
        }

        else if(state == 1) // enfant -> night, longfade "oops il fait déjà nuit, door (joueur rentre dedans sans dialog), invisible wall village
        {
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            playerStart.transform.position = spawnBeach.position;

            DialogOpen.instance.StartDialog(dialogState1);
            enabled = true;

            foreach (GameObject obj in triggerState1)
            {
                obj.SetActive(true);
            }

            skyLight.intensity = 0.2f;
        }

        else if (state == 2) // enfant -> night -> door ("J'entends des gens parler à l'interieur, je ne devrais pas rester là longtemps")
        {
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            playerStart.transform.position = spawnWindow.position;

            foreach (GameObject obj in triggerState2)
            {
                obj.SetActive(true);
            }
            skyLight.intensity = 0.2f;
            StartCoroutine(Fade());
        }

        else if(state == 3) // trigger boat -> direction lost island, door -> peut rentrer.
        {
            if (spawn.previousSceneName == "WoodenHut" && state == 3)
                playerStart.transform.position = spawnWoodenHut.position;

            foreach (GameObject obj in triggerState3)
            {
                obj.SetActive(true);
            }
            StartCoroutine(Fade());
        }
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

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}
