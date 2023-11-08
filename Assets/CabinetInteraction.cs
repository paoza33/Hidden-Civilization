using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinetInteraction : MonoBehaviour
{
    public string keyNeeded;
    public bool playerAlreadyInteract = false;
    
    public Dialog open;
    public Dialog notOpen;


    private void Awake()
    {
        enabled = false;
    }

    private void Update(){
        if(Input.GetButtonDown("Interact") && !playerAlreadyInteract){
            playerAlreadyInteract = true;
            if(Inventory.instance.FindItem(keyNeeded)){
                DialogOpen.instance.StartDialog(open);
                //rajouter flicker symbol
            }
            else{
                DialogOpen.instance.StartDialog(notOpen);
            }
        }
        else if(Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                playerAlreadyInteract = false;
                
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            enabled = false;
        }
    }
}