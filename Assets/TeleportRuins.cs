using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportRuins : MonoBehaviour
{
    public GameObject barrier;
    public Dialog dialogBarrier, dialogBarrierEN;
    public Light lt;
    public string levelToLoad;

    public AudioClip audioTeleport;

    private bool isEnglish;

    private void Awake()
    {
        isEnglish = LocaleSelector.instance.IsEnglish();
        enabled = false;
    }

    private void Update()
    {
        // couleur light
        lt.color -= (Color.white / 4f) * Time.deltaTime;
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
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogBarrierEN);
            else
                DialogOpen.instance.StartDialog(dialogBarrier);
            PlayerMovement.instance.enabled = true;
            GetComponent<BoxCollider>().enabled = false;
            enabled = true;
        }
        
    }

    public IEnumerator Fade()
    {
        enabled=false;
        PlayerMovement.instance.StopMovement();
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeIn");
        AudioManager.instance.StopCurrentSong();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayClipAt(audioTeleport, transform.position);

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        data.currentSceneName = "Tower";
        data.previousSceneName = "Ruins";
        SaveDataManager.SaveDataSpawn(data);

        SceneManager.LoadScene("Tower");
    }
}
