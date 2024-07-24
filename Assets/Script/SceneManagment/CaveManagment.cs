using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CaveManagment : MonoBehaviour
{
    public AudioClip audioClip;
    public Dialog intro, introEN;

    [HideInInspector]
    public bool isDialogOpen, isEnding;

    private bool isEnglish, canRestart;
    private TextMeshProUGUI textRestart, textInteract;

    public static CaveManagment instance;

    private void Awake()
    {
        if(instance != null){
            Debug.Log("Il y a plus d'une instance de CaveManagment");
            return;
        }
        instance = this;

        textRestart = GameObject.FindGameObjectWithTag("UIRestart").GetComponent<TextMeshProUGUI>();
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        isDialogOpen = true;
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>().enabled = false;
        isEnglish = LocaleSelector.instance.IsEnglish();
        if(isEnglish)
                DialogOpen.instance.StartDialog(introEN);
            else
                DialogOpen.instance.StartDialog(intro);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isDialogOpen)
        {
            if(!DialogOpen.instance.DisplayNextSentences())
            {
                if(!isEnding)
                    textRestart.enabled = true;
                isDialogOpen = false;
                StartCoroutine(Fade());
            }
        }
        if(Input.GetKeyDown(KeyCode.I) && !isDialogOpen){
            StartCoroutine(FadeRestart());
        }
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(audioClip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
    }

    private IEnumerator FadeRestart()
    {
        textInteract.enabled = false;
        textRestart.enabled = false;
        PlayerMovement.instance.StopMovement();
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Cave");
        
    }
}