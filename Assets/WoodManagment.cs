using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WoodManagment : MonoBehaviour
{
    public GameObject[] trees;

    public MeshRenderer Sphere;
    public Material[] materials;

    private bool flipflap;

    private TextMeshProUGUI textInteract;

    public bool isFirstStructInteraction;
    private bool isDialog;
    public Dialog firstInteract, firstInteractEN;

    private bool isEnglish;

    private void Awake()
    {
        isEnglish = LocaleSelector.instance.IsEnglish();
        enabled = false;
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if(isFirstStructInteraction){
                textInteract.enabled = false;
                TreeManagment();
                isFirstStructInteraction = false;
                if(isEnglish)
                    DialogOpen.instance.StartDialog(firstInteractEN);
                else
                    DialogOpen.instance.StartDialog(firstInteract);
                isDialog = true;
            }
            if(isDialog){
                if (!DialogOpen.instance.DisplayNextSentences()){
                    isDialog = false;
                    textInteract.enabled = true;
                }
            }
            else
                TreeManagment();
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
            textInteract.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
        textInteract.enabled = false;
    }

    public void TreeManagment()
    {
        foreach (GameObject tree in trees)
        {
            if(tree.activeSelf == true)
                tree.SetActive(false);
            else
                tree.SetActive(true);
        }
        if (flipflap)
        {
            Sphere.material = materials[1];
            flipflap = false;
        }
        else
        {
            Sphere.material = materials[0];
            flipflap = true;
        }
    }
}
