using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRotateBridge : MonoBehaviour
{
    public GameObject bridge;
    public Vector3 rotationEuler;
    public Vector3 rotationAxis;    // value x, y, z must be 0 or 1
    public float speed; // speed of rotation
    private Quaternion targetRotation;

    private bool firstEnter = true;

    private void Awake()
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        bridge.transform.rotation = Quaternion.Euler(bridge.transform.rotation.eulerAngles + rotationAxis * speed);
        if(Quaternion.Angle(bridge.transform.rotation, targetRotation) < 0.1f)
            enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firstEnter)
        {
            firstEnter = false;
            if (other.gameObject.CompareTag("Player"))
            {
                targetRotation = Quaternion.Euler(bridge.transform.rotation.eulerAngles + rotationEuler);
                enabled = true;
            }
        }          
    }
}
