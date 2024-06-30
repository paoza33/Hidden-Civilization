using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerDialog3 : MonoBehaviour
{
    public Dialog dialog, dialogEN;

    private Animator animator;

    private bool playerEnter;

    public bool unfixZ;
    public bool unfixX;

    public bool ifFade;

    public bool destroyTrigger;

    public AudioClip nightClip;
    public bool ifStopMusic;

    private bool isEnglish;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        isEnglish = LocaleSelector.instance.IsEnglish();
        enabled = false;
    }

    private void Update()
    {
        if (!playerEnter)
        {
            if(ifStopMusic)
                AudioManager.instance.StopCurrentSong();
            playerEnter = true;
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogEN);
            else
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
                if(destroyTrigger)
                {
                    Destroy(gameObject);
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
        enabled = false;
        PlayerMovement.instance.StopMovement();
        animator.SetTrigger("FadeIn");
        SaveDataSceneState data = SaveDataManager.LoadDataSceneState();
        data.campState = 1;
        SaveDataManager.SaveDataSceneState(data);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.StopCurrentSong();
        AudioManager.instance.PlayThemeSong(nightClip);
        SceneManager.LoadScene("Camp");
    }
}
