using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CityManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnHouse1;
    public Transform spawnHouse2;
    public Transform spawnHouse3;
    public Transform spawnRuins;
    public Transform spawnLibrary;
    public Transform spawnCityNight;

    public GameObject[] objState0;
    public GameObject[] objState1;
    public GameObject[] objState2;
    public GameObject[] objState3;

    public Light skyLight;

    private int state;

    public AudioClip audioClipDay, audioClipNight;
    private bool isNight;

    public Dialog dialogNight, dialogNightEN;

    private bool isEnglish;

    private void Awake()
    {
        enabled = false;
        isEnglish = LocaleSelector.instance.IsEnglish();

        state = SaveDataManager.LoadDataSceneState().cityState;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "House1")
            playerStart.transform.position = spawnHouse1.position;

        else if (data.previousSceneName == "House2")
            playerStart.transform.position = spawnHouse2.position;

        else if (data.previousSceneName == "House3")
            playerStart.transform.position = spawnHouse3.position;

        else if (data.previousSceneName == "Ruins")
            playerStart.transform.position = spawnRuins.position;

        else if (data.previousSceneName == "Library")
            playerStart.transform.position = spawnLibrary.position;

        SettingsCityState();

        StartCoroutine(Fade());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                enabled = false;
                Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                animator.SetTrigger("FadeOut");
            }
        }
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
        if (isNight){
            AudioManager.instance.PlayThemeSong(audioClipNight);
            if (isEnglish)
            {
                DialogOpen.instance.StartDialog(dialogNightEN);
            }
            else
                DialogOpen.instance.StartDialog(dialogNight);

            enabled = true;

        }
            
        else{
            AudioManager.instance.PlayThemeSong(audioClipDay);
            Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
            animator.SetTrigger("FadeOut");
        }
    }

    private void SettingsCityState()
    {
        if (state == 0)  // direction library
        {
            isNight = false;
            foreach (GameObject obj in objState0)
            {
                obj.SetActive(true);
            }
        }
        else if (state == 1)    // direction library night
        {
            playerStart.transform.position = spawnCityNight.position;
            foreach (GameObject obj in objState1)
            { obj.SetActive(true); }

            skyLight.intensity = 0.2f;
            isNight = true;
        }
        else if (state == 2)    // objectif ruins
        {
            foreach (GameObject obj in objState2)
            { obj.SetActive(true); }
            isNight = false;
        }
        else if (state == 3)    // retour ruins
        {
            foreach (GameObject obj in objState3)
            { obj.SetActive(true); }
            isNight = false;
        }
    }
}