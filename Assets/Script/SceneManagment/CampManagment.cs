using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnWoodenHut;
    public Transform spawnBeach;
    public Transform spawnWindow;
    private int state;
    public GameObject[] SpecialTrees;
    public Light skyLight;

    public GameObject[] triggerBoat;
    public GameObject triggerBeach;

    private void Awake()
    {
        state = SaveDataManager.LoadDataSceneState().campState;
        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "WoodenHut" && state ==2)     // modifier de telle façon que state 0 -> cabane impossible, soleil, state 1 = tomber nuit cabane possible, state 2 nuit et on pars, state 3 present
            playerStart.transform.position = spawnWoodenHut.position;

        else if(data.previousSceneName == "WoodenHut" && state == 1)
            playerStart.transform.position = spawnWindow.position;

        else if(data.previousSceneName == "Camp")
            playerStart.transform.position = spawnBeach.position;
        
    }

    private void Start()
    {
        if(state == 0)
        {
            foreach(GameObject obj in SpecialTrees)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in triggerBoat)
                obj.SetActive(false);
        }
        else if(state == 1)
        {
            foreach(GameObject obj in SpecialTrees)
            {
                obj.SetActive(true);
            }

            triggerBeach.SetActive(false);

            foreach (GameObject obj in triggerBoat)
                obj.SetActive(false);

            skyLight.intensity = 0.2f;
        }
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
