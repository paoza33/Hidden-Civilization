using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KnowledgePlaceManagment : MonoBehaviour
{
    private GameObject player;
    public GameObject unknown;
    public Dialog firstMoment, lastSpeech;
    public Transform secondaryCam;

    private bool ending;

    public AudioClip clip;

    public static KnowledgePlaceManagment instance;

    private void Awake()
    {
        enabled = false;

        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FadeOutState0());    

        if(instance != null)
        {
            Debug.Log("Il y a plus d'une instance de KnowledgePlaceManagment");
            return;
        }
        instance = this;
    }

    private void Update(){
        if(Input.GetButtonDown("Interact")){
            if(!DialogOpen.instance.DisplayNextSentences()){
                if(!ending)
                    enabled = false;
                else
                    StartCoroutine(FadeIn());
            }
        }
    }

    private IEnumerator FadeOutState0()
    {
        PlayerMovement.instance.StopMovement();

        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(clip);
        DialogOpen.instance.StartDialog(firstMoment);

        enabled = true;

        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }

    public void Transition()
    {
        AudioManager.instance.StopCurrentSong();
        StartCoroutine(FadeOutState1());
    }

    private IEnumerator FadeOutState1()
    {
        CameraMovement.instance.enabled = false;
        player.SetActive(false);
        unknown.SetActive(true);

        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(0.2f);
        Camera.main.transform.position = secondaryCam.position;
        Camera.main.transform.rotation = secondaryCam.rotation;

        yield return new WaitForSeconds(0.8f);
        DialogOpen.instance.StartDialog(lastSpeech);
        ending = true;
        enabled = true;

        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }

    private IEnumerator FadeIn(){
        Debug.Log("fade in");
        PlayerMovement.instance.StopMovement();
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Credit");
    }
}
