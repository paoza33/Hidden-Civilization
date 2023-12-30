using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinetInteraction : MonoBehaviour
{
    public string keyNeeded;
    private bool playerAlreadyInteract = false;
    private bool flickeringActivate = false;
    
    public Dialog open;
    public Dialog notOpen;

    public GameObject symbol;

    private void Awake()
    {
        enabled = false;
    }
    
    private void Update(){
        if(Input.GetButtonDown("Interact") && !playerAlreadyInteract){
            playerAlreadyInteract = true;
            if(Inventory.instance.FindItem(keyNeeded)){
                DialogOpen.instance.StartDialog(open);
                if (!flickeringActivate)
                {
                    flickeringActivate = true;
                    symbol.GetComponent<FlickeringEmissive>().isReverse = false;
                    symbol.GetComponent<FlickeringEmissive>().enabled = true;
                }
                
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
