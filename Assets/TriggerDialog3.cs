using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerDialog3 : MonoBehaviour
{
    public Dialog dialog;

    private Animator animator;

    private bool playerEnter;

    public bool unfixZ;
    public bool unfixX;

    public bool ifFade;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        enabled = false;
    }

    private void Update()
    {
        if (!playerEnter)
        {
            playerEnter = true;
            DialogOpen.instance.StartDialog(dialog);
        }
        else if(playerEnter && Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                if (ifFade)
                {
                    StartCoroutine(FadeCamp());
                }
                else
                {
                    PlayerMovement.instance.enabled = true;
                    if (unfixX)
                        CameraMovement.instance.cameraFixX = false;
                    if (unfixZ)
                        CameraMovement.instance.cameraFixZ = false;
                    enabled = false;
                }                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
            playerEnter = false;
        }
    }

    private IEnumerator FadeCamp()
    {
        PlayerMovement.instance.StopMovement();
        animator.SetTrigger("FadeIn");
        SaveDataSceneState data = SaveDataManager.LoadDataSceneState();
        data.campState = 1;
        SaveDataManager.SaveDataSceneState(data);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Camp");
    }
}
