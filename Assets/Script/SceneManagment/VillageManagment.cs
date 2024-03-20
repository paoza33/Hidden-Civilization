using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnHome, spawnCity, spawnCamp, spawnWood;

    public Light skyLight;

    private int state;

    public GameObject[] colliderState0, colliderState1, colliderState2, colliderState3;

    public GameObject[] portalsState0, portalsState1, portalsState2, portalsState3, portalsState4;

    private void Awake()
    {
        SaveDataSceneState dataState = SaveDataManager.LoadDataSceneState();
        state = dataState.villageState;
        if(state == 0)  // joueur enfant, il se dirige vers le camp, on bloque wood et city
        {
            // enfant jour avec objectif -> camp
            foreach(GameObject obj in colliderState0)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState0)
                obj.SetActive(true);
            
        }
        else if(state == 1){
            // enfant nuit objectif -> home, on bloque city, wood et camp
            skyLight.intensity = 0.2f;
            foreach (GameObject obj in colliderState1)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState1)
                obj.SetActive(true);
        }
        else if(state == 2) // joueur se dirige vers city, on ne bloque rien
        {
            foreach (GameObject obj in colliderState2)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState2)
                obj.SetActive(true);
        }
        else if(state == 3){    // joueur se dirige vers wood, nuit, on bloque city et camp
            skyLight.intensity = 0.2f;
            foreach (GameObject obj in colliderState3)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState3)
                obj.SetActive(true);
        }
        else // default bloque rien
        {
            //present
            foreach(GameObject obj in portalsState4)
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
