using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportRuins : MonoBehaviour
{
    public GameObject barrier;
    public Dialog dialogBarrier;
    public Animator animator;
    public string levelToLoad;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        enabled = false;
    }

    private void Update()
    {
        animator.SetTrigger("FadeRedIn");
        if(Input.GetButtonDown("Interact")){
            if(!DialogOpen.instance.DisplayNextSentences()){
                PlayerMovement.instance.StopMovement();
                StartCoroutine(Fade());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            barrier.SetActive(true);
            DialogOpen.instance.StartDialog(dialogBarrier);
            PlayerMovement.instance.enabled = true;
            GetComponent<BoxCollider>().enabled = false;
            enabled = true;
        }
        
    }

    public IEnumerator Fade()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.50f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
