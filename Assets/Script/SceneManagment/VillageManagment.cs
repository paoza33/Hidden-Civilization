using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnHome;
    public Transform spawnCity;
    public Transform spawnCamp;
    public Transform spawnWood;

    private void Awake()
    {
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
