using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManagment : MonoBehaviour
{
    public AudioClip audioClip;
    public Dialog intro, introEN;
    private bool isEnglish;

    private void Awake()
    {
        isEnglish = LocaleSelector.instance.IsEnglish();
        if(isEnglish)
                DialogOpen.instance.StartDialog(introEN);
            else
                DialogOpen.instance.StartDialog(intro);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if(!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(Fade());
                enabled = false;
            }
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
}