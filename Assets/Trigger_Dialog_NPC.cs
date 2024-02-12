using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog_NPC : MonoBehaviour
{
    public Animator animatorNPC;

    public bool ifAnimationBefore;
    public bool ifAnimationAfter;
    public bool playerAlreadyInteract;



    public Dialog dialogNPC;
    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact") && !playerAlreadyInteract){
            playerAlreadyInteract = true;
            DialogOpen.instance.StartDialog(dialogNPC);
        }
        else if(Input.GetButtonDown("Interact")){
            if (!DialogOpen.instance.DisplayNextSentences()){
                playerAlreadyInteract = false;

            }
        }
        /*
            interaction du joueur
            ouverture dialog
            vérifier si il y a une animation du PNJ (si il court on arrête)
            fin du dialog
            si animation stoppé ou lancer -> faire l'action

        */
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            enabled = true;
        }
    }
}
