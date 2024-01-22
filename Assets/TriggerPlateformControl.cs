using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlateformControl : MonoBehaviour
{
    public string color;

    [SerializeField]
    private GameObject [] buttons;

    [SerializeField]
    private LineRenderer laser;
    [SerializeField]
    private Transform [] startPoint;
    [SerializeField]
    private Transform[] endPoints;
    private int indexPoints = 0;

    [SerializeField]
    private float speed;

    private bool canInteract = false;
    private bool firstInteract = true;
    private bool canChangeButton = true;

    private void Awake()
    {
        enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttons[indexPoints].GetComponent<FlickeringEmissive>().isReverse = false;
            buttons[indexPoints].GetComponent<FlickeringEmissive>().enabled = true;
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttons[indexPoints].GetComponent<FlickeringEmissive>().isReverse = true;
            buttons[indexPoints].GetComponent<FlickeringEmissive>().enabled = true;
            enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && canInteract)
        {
            canChangeButton = true;
        }
        if (Input.GetButtonDown("Interact") && firstInteract)
        {
            PlayerMovement.instance.StopMovement();
            LaserSphereManagment.instance.SetLaser(laser, startPoint[indexPoints], endPoints[indexPoints]);
            laser.enabled = true;

            canInteract = true;
            firstInteract = false;
        }

        else if (Input.GetButtonDown("Interact") && canInteract)
        {
            PlateformMovement.instance.Moove(color, indexPoints);
        }
        else if (Input.GetAxisRaw("Horizontal") == 1 && canInteract && canChangeButton)
        {
            canChangeButton = false;
            if (!(indexPoints +1 >= endPoints.Length))
            {
                buttons[indexPoints +1].GetComponent<FlickeringEmissive>().isReverse = false;
                buttons[indexPoints +1].GetComponent<FlickeringEmissive>().enabled = true;
                buttons[indexPoints].GetComponent<FlickeringEmissive>().isReverse = true;
                buttons[indexPoints].GetComponent<FlickeringEmissive>().enabled = true;

                indexPoints ++;
                LaserSphereManagment.instance.SetLaser(laser, startPoint[indexPoints], endPoints[indexPoints]);
            }          
        }
        else if(Input.GetAxisRaw("Horizontal") == -1 && canInteract && canChangeButton)
        {
            canChangeButton = false;
            if (!(indexPoints -1 <0))
            {
                buttons[indexPoints - 1].GetComponent<FlickeringEmissive>().isReverse = false;
                buttons[indexPoints - 1].GetComponent<FlickeringEmissive>().enabled = true;
                buttons[indexPoints].GetComponent<FlickeringEmissive>().isReverse = true;
                buttons[indexPoints].GetComponent<FlickeringEmissive>().enabled = true;

                indexPoints --;
                LaserSphereManagment.instance.SetLaser(laser, startPoint[indexPoints], endPoints[indexPoints]);
            }
        }
        else if(Input.GetButtonDown("Cancel") && canInteract)
        {
            PlayerMovement.instance.enabled = true;
            laser.enabled = false;

            canInteract=false;
            firstInteract = true;
        }
    }
}