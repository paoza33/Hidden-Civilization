using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyFadeOut : MonoBehaviour
{
    public bool unfixX;
    public bool unfixZ;
    
    public AudioClip clip;

    private void Start()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(clip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        if (unfixX)
            CameraMovement.instance.cameraFixX = false;
        if (unfixZ)
            CameraMovement.instance.cameraFixZ = false;
    }
}
