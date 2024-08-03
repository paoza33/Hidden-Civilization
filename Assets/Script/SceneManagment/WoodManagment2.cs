using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodManagment2 : MonoBehaviour
{
    public GameObject playerStart;
    public Transform spawnCave;

    private int state;

    public GameObject[] objState0;
    public GameObject[] objState1;

    public Light[] lights;

    private bool isNight;


    public AudioClip clipDay, clipEnigm;

    public Animator secretDoor;

    public Dialog intro, introEN;

    public static WoodManagment2 instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else{
            Debug.Log("Il y a plus d'une instance de WoodManagment2");
            return;
        }

        state = SaveDataManager.LoadDataSceneState().woodState;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Cave")
            playerStart.transform.position = spawnCave.position;

        if (state == 0)
        {
            isNight = false;
            foreach (GameObject obj in objState0)
                obj.SetActive(true);
        }
        else if (state == 1)
        {
            isNight = true;
            foreach (Light light in lights)
                light.intensity = 0;

            if (LocaleSelector.instance.IsEnglish())
                DialogOpen.instance.StartDialog(introEN);
            else
                DialogOpen.instance.StartDialog(intro);

            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>().enabled = true;
            foreach (GameObject obj in objState1)
                obj.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                enabled = false;
                StartCoroutine(Fade());
            }
        }
    }

    public void OpenSecretDoor(){
        secretDoor.SetBool("PlayerHaveKey", true);
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);

        if(isNight)
            AudioManager.instance.PlayThemeSong(clipEnigm);
        else
            AudioManager.instance.PlayThemeSong(clipDay);

        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }

}


