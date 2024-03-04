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
    private void Awake()
    {
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
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }
}
