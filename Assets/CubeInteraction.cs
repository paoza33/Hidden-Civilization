using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteraction : MonoBehaviour
{
    public CubeObject cube;
    private void Awake()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enabled=true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CubeMovement.instance.Moove(cube, name); // ne marche pas car on ne sait pas la position du cube dans la grid
            enabled = false;
        }
    }
}
