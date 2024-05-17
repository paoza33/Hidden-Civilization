using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KnowledgePlaceManagment : MonoBehaviour
{
    private GameObject player;
    public GameObject unknown;
    public Dialog firstMoment, lastSpeech;
    private int state;
    public Transform secondaryCam;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        SaveDataSceneState data = SaveDataManager.LoadDataSceneState();
        state = data.knowledgePlaceState;

        if(state == 0){
            DialogOpen.instance.StartDialog(firstMoment);
        }
        else if(state == 1){
            CameraMovement.instance.enabled = false;

            unknown.SetActive(true);
            player.SetActive(false);

            DialogOpen.instance.StartDialog(lastSpeech);
            Camera.main.transform.position = secondaryCam.position; // chercher pourquoi main cam reste au même endroit
            Camera.main.transform.rotation = secondaryCam.rotation;
        }

        StartCoroutine(FadeOut());
    }

    private void Update(){
        if(Input.GetButtonDown("Interact")){
            if(DialogOpen.instance.DisplayNextSentences()){
                PlayerMovement.instance.enabled = true;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }

    private IEnumerator FadeIn(){
        PlayerMovement.instance.StopMovement();
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Credit");
    }
}
