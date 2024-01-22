using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFracture : MonoBehaviour
{
    public bool grounded;
    public bool connected;

    public List<SubFracture> connections;

    private Fracture parent;

    public bool isConnexionNeedToBeBreak;

    private void Start()
    {
        //parent = transform.root.GetComponent<Fracture>();
        GetComponent<Rigidbody>().isKinematic = true;
        enabled = false;
    }

    void Update()
    {
        /*for (int i = 0; i < connections.Count; i++) // Make sure it is not isolated
        {
            if (!connections[i].grounded && !connections[i].connected)
            {
                connections.Remove(connections[i]);
            }
        }

        bool somehowGrounded = false;

        for (int i = 0; i < connections.Count; i++) // Make sure it is connect to a ground some way or another
        {
            if (connections[i].grounded)
            {
                somehowGrounded = true;
                break;
            }

            for (int i2 = 0; i2 < connections[i].connections.Count; i2++) // Check one more iteration through
            {
                if (connections[i].connections[i2].grounded)
                {
                    somehowGrounded = true;
                    break;
                }
            }
        }
        if (isConnexionNeedToBeBreak)
            connected = false;
        else
            connected = somehowGrounded && connections.Count >= 1 || grounded;
        GetComponent<Rigidbody>().isKinematic = connected;*/

        if (isConnexionNeedToBeBreak)
            connected = false;
        else
            connected = true;
        GetComponent<Rigidbody>().isKinematic = connected;
    }

    /*void OnCollisionEnter(Collision collision)        // needed for collision
    {
        if (collision.impulse.magnitude > parent.breakForce)
        {
            // Make sure the hit cell disconnects
            connections = new List<SubFracture>();
            grounded = false;

            parent.FractureFonction(collision.contacts[0].point, collision.impulse);
        }
    }*/

    public void BreakAllConnexion()
    {
        isConnexionNeedToBeBreak = true;
    }

    public void ReconnectAllConnexion()
    {
        isConnexionNeedToBeBreak = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            Gizmos.DrawLine(transform.position, connections[i].transform.position);
        }
    }
}
