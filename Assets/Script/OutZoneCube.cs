using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutZoneCube : MonoBehaviour
{
    public Shader shader;
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
        collision.gameObject.GetComponent<MeshRenderer>().material.shader = shader;
        DissolveElement.instance.Dissolve(collision.gameObject.GetComponent<MeshRenderer>().material, "_Dissolve_value", collision.gameObject.GetComponent<CubeObject>());
    }
}
