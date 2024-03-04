using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodManagment2 : MonoBehaviour
{
    public GameObject playerStart;
    public Transform spawnCave;

    private void Awake()
    {
        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Cave")
            playerStart.transform.position = spawnCave.position;
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


