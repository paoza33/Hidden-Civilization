using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlateformEnvironment : MonoBehaviour
{
    public PhysicMaterial defaultMaterial;
    public PhysicMaterial noFrictionMaterial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BoxCollider>().material = defaultMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BoxCollider>().material = noFrictionMaterial;
        }
    }
}
