using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinetInteraction : MonoBehaviour
{
    public string keyNeeded;
    public bool playerAlreadyInteract = false;
    
    public Dialog open;
    public Dialog notOpen;

    public int id;

    public GameObject symbol;

    private TextMeshProUGUI textInteract;
    private TextMeshProUGUI readBook;

    private void Awake()
    {
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        readBook = GameObject.FindGameObjectWithTag("UIReadBook").GetComponent<TextMeshProUGUI>();
        enabled = false;
    }
    
    private void Update(){
        if(Input.GetButtonDown("Interact") && !playerAlreadyInteract){
            LibraryManagment.instance.anotherInteraction = true;
            playerAlreadyInteract = true;
            textInteract.enabled = false;
            readBook.enabled = false;
            LibraryManagment.instance.UpdateCurrentCabinet(id, symbol, GetComponent<BoxCollider>());

            DialogOpen.instance.StartDialog(open);
        }
        else if(Input.GetButtonDown("Interact"))
        {
            if(!DialogOpen.instance.DisplayNextSentences()){
                LibraryManagment.instance.anotherInteraction = false;
                enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            textInteract.enabled = true;
            enabled = true;
            LibraryManagment.instance.enabled = false;
            readBook.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            textInteract.enabled = false;
            enabled = false;
            LibraryManagment.instance.enabled = true;
            readBook.enabled = true;
        }
    }
}