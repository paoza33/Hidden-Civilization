using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library2Managment : MonoBehaviour
{
    public Dialog dialog, dialogEN;
    private Animator animator;

    private bool isEnglish;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        isEnglish = LocaleSelector.instance.IsEnglish();
        if(isEnglish)
                DialogOpen.instance.StartDialog(dialogEN);
        else
            DialogOpen.instance.StartDialog(dialog);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")){
            if(!DialogOpen.instance.DisplayNextSentences()){
                StartCoroutine(StartingFade());
            }
        }
    }

    private IEnumerator StartingFade()
    {
        PlayerMovement.instance.StopMovement();
        enabled = false;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
    }
}
