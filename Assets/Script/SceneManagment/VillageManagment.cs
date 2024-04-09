using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageManagment : MonoBehaviour
{
    public GameObject playerStart;
    private GameObject player;

    public Transform spawnHome, spawnCity, spawnCamp, spawnWood;

    public Light skyLight;

    private int state;

    public GameObject[] colliderState0, colliderState1, colliderState2, colliderState3, colliderState4, colliderState5, colliderState6, colliderState7;

    public GameObject[] portalsState0, portalsState1, portalsState2, portalsState3, portalsState4, portalsState5, portalsState6, portalsState7;

    private void Awake()
    {
        enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");

        SaveDataSceneState dataState = SaveDataManager.LoadDataSceneState();
        state = dataState.villageState;
        Debug.Log("state village = " + state);
        if(state == 0)  // joueur enfant, il se dirige vers le camp, on bloque wood et city
        {
            enabled = true;
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            // enfant jour avec objectif -> camp
            foreach (GameObject obj in colliderState0)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState0)
                obj.SetActive(true);
        }
        else if(state == 1){
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            // enfant nuit objectif -> home, on bloque city, wood et camp
            skyLight.intensity = 0.2f;
            foreach (GameObject obj in colliderState1)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState1)
                obj.SetActive(true);
        }
        else if(state == 2) // joueur se dirige vers city, on bloque wood et camp
        {
            foreach (GameObject obj in colliderState2)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState2)
                obj.SetActive(true);
        }
        else if (state == 3) // joueur se dirige vers wood en journée, on bloque City et camp
        {
            foreach (GameObject obj in colliderState3)
                obj.SetActive(true);
            foreach (GameObject obj in portalsState3)
                obj.SetActive(true);
        }
        else if(state == 4) // retour wood, direction home pour dormir
        {
            foreach (GameObject obj in colliderState4)
                obj.SetActive(true);
            foreach (GameObject obj in portalsState4)
                obj.SetActive(true);
        }
        else if(state == 5){    // joueur se dirige vers wood, nuit, on bloque city et camp
            skyLight.intensity = 0.2f;
            foreach (GameObject obj in colliderState5)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState5)
                obj.SetActive(true);
        }
        else if(state == 6){        // retour wood, direction city -> objectif ruins, on bloque wood et camp
            foreach (GameObject obj in colliderState6)
                obj.SetActive(true);
            foreach(GameObject obj in portalsState6)
                obj.SetActive(true);
        }
        else if (state == 7)    // retour ruins, objectif lost island, on bloque city, wood
        {
            foreach (GameObject obj in colliderState7)
                obj.SetActive(true);
            foreach (GameObject obj in portalsState7)
                obj.SetActive(true);
        }

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();                   // changer le fait que lorsqu'on interagit avec l'homme msiterieux et qu'on va dans le village, il fait nuit et on peut rentrer dans wood
        if (data.previousSceneName == "Home")
            playerStart.transform.position = spawnHome.position;

        else if (data.previousSceneName == "City")
            playerStart.transform.position = spawnCity.position;

        else if (data.previousSceneName == "Camp")
            playerStart.transform.position = spawnCamp.position;

        else if(data.previousSceneName == "Wood")
            playerStart.transform.position = spawnWood.position;

        StartCoroutine(Fade());
    }

    private void Update()   // update enabled only in state 0
    {
        if (Input.GetButtonDown("Map"))
        {
            GameObject.FindGameObjectWithTag("UIMapText").GetComponent<TextMeshProUGUI>().enabled = false;
            enabled = false;
        }
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        if(state == 0){
            GameObject.FindGameObjectWithTag("UIMapText").GetComponent<TextMeshProUGUI>().enabled = true;
            enabled = true;
        }
        else
        {
            PlayerMovement.instance.enabled = true;
            CameraMovement.instance.cameraFixX = false;
            CameraMovement.instance.cameraFixZ = false;
        }       
    }
}
