using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WoodManagment : MonoBehaviour
{
    public GameObject[] trees;

    public MeshRenderer Sphere;
    public Material[] materials;

    private bool flipflap;

    private void Awake()
    {
        enabled = false;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
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
