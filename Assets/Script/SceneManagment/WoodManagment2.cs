using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodManagment2 : MonoBehaviour
{
    public GameObject playerStart;
    public Transform spawnCave;

    private int state;

    public GameObject[] triggerState0;
    private GameObject[] triggerState1;

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
        if(state == 0)
        {
            foreach(GameObject obj in triggerState0)
                obj.SetActive(true);
        }
        else if(state == 1)
        {
            foreach (GameObject obj in triggerState1)
                obj.SetActive(true);
        }
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


