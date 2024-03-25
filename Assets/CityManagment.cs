using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnHouse1;
    public Transform spawnHouse2;
    public Transform spawnHouse3;
    public Transform spawnRuins;
    public Transform spawnLibrary;

    public GameObject[] objState0;
    public GameObject[] objState1;
    public GameObject[] objState2;
    public GameObject[] objState3;

    public Light skyLight;

    private int state;

    private void Awake()
    {
        state = SaveDataManager.LoadDataSceneState().cityState;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "House1")
            playerStart.transform.position = spawnHouse1.position;

        else if (data.previousSceneName == "House2")
            playerStart.transform.position = spawnHouse2.position;

        else if (data.previousSceneName == "House3")
            playerStart.transform.position = spawnHouse3.position;

        else if (data.previousSceneName == "Ruins")
            playerStart.transform.position = spawnRuins.position;

        else if (data.previousSceneName == "Library")
            playerStart.transform.position = spawnLibrary.position;
    }
    private void Start()
    {
        if(state == 0)
        {
            foreach(GameObject obj in objState0)
            {
                obj.SetActive(true);
            }
        }
        else if(state == 1)
        {
            foreach(GameObject obj in objState1)
            { obj.SetActive(true); }

            skyLight.intensity = 0.2f;
        }
        else if( state == 2)    // library close (cambriolage)
        {
            foreach (GameObject obj in objState2)
            { obj.SetActive(true); }
        }
        else if(state == 3)
        {
            foreach (GameObject obj in objState3)
            { obj.SetActive(true); }
        }

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }
}