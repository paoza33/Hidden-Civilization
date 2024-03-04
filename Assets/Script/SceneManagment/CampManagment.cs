using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnWoodenHut;
    public Transform spawnBeach;
    private int state;
    public GameObject[] SpecialTrees;
    public Light skyLight;

    public GameObject[] triggerBoat;
    public GameObject triggerBeach;

    private void Awake()
    {
        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "WoodenHut")
            playerStart.transform.position = spawnWoodenHut.position;
        else if(data.previousSceneName == "Camp")
            playerStart.transform.position = spawnBeach.position;
        state = SaveDataManager.LoadDataSceneState().campState;
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
