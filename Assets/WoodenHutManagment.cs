using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WoodenHutManagment : MonoBehaviour
{
    public Vector3 cameraMainPos;

    public GameObject[] objState0;
    public GameObject[] objState1;

    private SaveDataSceneState data;
    private void Start()
    {
        data = SaveDataManager.LoadDataSceneState();
        StartCoroutine(Fade());
        if(data.woodenHutState == 0)
        {
            foreach(GameObject obj in objState0)
            {
                obj.SetActive(true);
            }
        }
        else if(data.woodenHutState == 1)
        {
            foreach (GameObject obj in objState1)
            {
                obj.SetActive(true);
            }
        }
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(0.1f);
        CameraMovement.instance.cameraFixX = true;
        Camera.main.transform.position = cameraMainPos; // solution rapide car bug lorsqu'on lance le jeu sur cette map

        yield return new WaitForSeconds(0.9f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
    }
}
