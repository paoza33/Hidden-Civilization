using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnHome, spawnCity, spawnCamp, spawnWood;

    public Light skyLight;

    private int state;

    public GameObject[] colliderState0, colliderState1, colliderState2;

    private void Awake()
    {
        SaveDataSceneState dataState = SaveDataManager.LoadDataSceneState();
        state = dataState.villageState;
        if(state == 0)
        {
            // enfant jour avec objectif -> camp
            foreach(GameObject obj in colliderState0)
                obj.SetActive(true);
        }
        else if(state == 1)
        {
            // enfant nuit objectif -> home
            skyLight.intensity = 0.2f;
            foreach (GameObject obj in colliderState1)
                obj.SetActive(true);
        }
        else
        {
            //present
            foreach (GameObject obj in colliderState2)
                obj.SetActive(true);
        }


        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Home")
            playerStart.transform.position = spawnHome.position;

        else if (data.previousSceneName == "City")
            playerStart.transform.position = spawnCity.position;

        else if (data.previousSceneName == "Camp")
            playerStart.transform.position = spawnCamp.position;

        else if(data.previousSceneName == "Wood")
            playerStart.transform.position = spawnWood.position;
    }
    private void Start()
    {
        StartCoroutine(Fade());
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
