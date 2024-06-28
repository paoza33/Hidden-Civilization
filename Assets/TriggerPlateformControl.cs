using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public bool iftuto1, iftuto2;

    private TextMeshProUGUI textInteract;

    private void Awake()
    {
        enabled = false;
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInteract.enabled = true;
            buttons[indexPoints].GetComponent<FlickeringEmissive>().isReverse = false;
            buttons[indexPoints].GetComponent<FlickeringEmissive>().enabled = true;
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInteract.enabled = false;
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
            if (iftuto1)
                textInteract.text = "E TO TURN THE PILLAR";
            else if (iftuto2)
                textInteract.text = "LEFT OR RIGHT TO CHANGE THE PILLARS";
            else
                textInteract.enabled = false;
            PlayerMovement.instance.StopMovement();
            LaserSphereManagment.instance.SetLaser(laser, startPoint[indexPoints], endPoints[indexPoints]);
            laser.enabled = true;

            canInteract = true;
            firstInteract = false;
        }

        else if (Input.GetButtonDown("Interact") && canInteract)
        {
            PlateformMovement.instance.Moove(color, indexPoints);
            if (iftuto1)
                textInteract.text = "ESCAPE TO QUIT";
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
            textInteract.text = "E TO INTERACT";
            textInteract.enabled = true;
            PlayerMovement.instance.enabled = true;
            laser.enabled = false;

            canInteract=false;
            firstInteract = true;
        }
    }
}