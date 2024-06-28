using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WoodenHutManagment : MonoBehaviour
{
    public Material[] youngSkin;

    public Vector3 cameraMainPos;

    public GameObject[] objState0;
    public GameObject[] objState1;

    private GameObject player;

    private SaveDataSceneState data;

    public AudioClip clip;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        data = SaveDataManager.LoadDataSceneState();

        if(data.woodenHutState == 0)
        {
            player.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            Material[] tempMat = player.GetComponentInChildren<SkinnedMeshRenderer>().materials;

            for (int i = 0; i < tempMat.Length; i++)
            {
                tempMat[i].color = youngSkin[i].color;
            }

            foreach (GameObject obj in objState0)
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

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        CameraMovement.instance.cameraFixX = true;
        CameraMovement.instance.fixX = cameraMainPos.x;
        Camera.main.transform.position = cameraMainPos; // solution rapide car bug lorsqu'on lance le jeu sur cette map

        yield return new WaitForSeconds(0.1f);

        PlayerMovement.instance.StopMovement();

        yield return new WaitForSeconds(1f);

        AudioManager.instance.PlayThemeSong(clip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
    }
}
