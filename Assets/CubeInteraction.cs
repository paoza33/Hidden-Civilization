using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeInteraction : MonoBehaviour
{
    public CubeObject cube;
    private TextMeshProUGUI textInteract;
    private void Awake()
    {
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            enabled = true;
            textInteract.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            enabled=false;
            textInteract.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            textInteract.enabled = false;
            CubeMovement.instance.Moove(cube, name); // ne marche pas car on ne sait pas la position du cube dans la grid
            enabled = false;
        }
    }
}
