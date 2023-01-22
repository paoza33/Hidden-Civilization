using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerChangeScene : MonoBehaviour
{
    public string levelToLoad;
    private Animator animator;
    private bool alreadyTriggered;
    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        alreadyTriggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyTriggered)
        {
            PlayerMovement.instance.enabled = false;
            alreadyTriggered = true;
            StartCoroutine(Fade());
        }
    }
    public IEnumerator Fade()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(levelToLoad);
    }
}
