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

    private void Awake()
    {
        enabled = false;
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
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
