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
    private void Awake()
    {
        state = SaveDataManager.LoadDataSceneState().woodState;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Cave")
            playerStart.transform.position = spawnCave.position;
        StartCoroutine(Fade());
    }
    private void Start()
    {
        if (state == 0)
        {
            isNight = false;
            foreach (GameObject obj in objState0)
                obj.SetActive(true);
        }
        else if(state == 1)
        {
            isNight = true;
            foreach (Light light in lights)
                light.intensity = 0;

            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>().enabled = true;
            foreach (GameObject obj in objState1)
                obj.SetActive(true);           
        }
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


