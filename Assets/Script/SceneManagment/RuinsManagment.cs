using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnTower;
    public GameObject[] objState0;
    public GameObject[] objState1;


    private void Awake()
    {
        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Tower")
            playerStart.transform.position = spawnTower.position;
        
        SaveDataSceneState state = SaveDataManager.LoadDataSceneState();
        if(state.ruinsState == 0){
            foreach(GameObject obj in objState0)
                obj.SetActive(true);
        }
        else if(state.ruinsState == 1){
            foreach(GameObject obj in objState0)
                obj.SetActive(true);
        }
        

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
