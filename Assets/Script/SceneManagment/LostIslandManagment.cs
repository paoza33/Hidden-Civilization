using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostIslandManagment : MonoBehaviour
{
    public AudioClip clip;
    public Dialog dialog, dialogEN;

    private void Awake()
    {
        bool isEnglish = LocaleSelector.instance.IsEnglish();
        if(isEnglish)
            DialogOpen.instance.StartDialog(dialogEN);
        else
            DialogOpen.instance.StartDialog(dialog);
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(Fade());
            }
        }
    }

    private IEnumerator Fade()
    {
        enabled = false;
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(clip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
    }
}
