using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManagment : MonoBehaviour
{
    public AudioClip audioClip;
    private void Awake()
    {
        AudioManager.instance.PlayThemeSong(audioClip);
        Animator credit = GameObject.FindGameObjectWithTag("Credit").GetComponent<Animator>();
        credit.SetTrigger("CreditIn");
    }

    private void Update()
    {
        if(Input.GetButtonDown("Escape")){
            Application.Quit();
        }
    }
}
