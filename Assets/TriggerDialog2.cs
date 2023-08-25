using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDialog2 : MonoBehaviour
{
    private bool playerAlreadyInteract;
    private bool canDisplayNextSentences = true;

    private int index = 0;  //index of contenair's list


    public Dialog[] dialogs;
    public Dialog emptyDialog;

    public GameObject[] contenairs;
    private Vector3[] startPosContenairs;   // contain all contenairs position before next movement

    private bool moovingLeft, moovingRight;
    private bool isMooving = false;

    public float speedDeplacement = 0;

    private void Awake()
    {
        enabled = false;

        startPosContenairs = new Vector3[contenairs.Length];

        for(int i = 0; i < startPosContenairs.Length; i++)
        {
            startPosContenairs[i] = contenairs[i].transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)
        {
            playerAlreadyInteract = true;
            DialogOpen.instance.StartDialog(dialogs[index]);
        }
        else if (Input.GetButtonDown("Interact") && canDisplayNextSentences)
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                playerAlreadyInteract = false;
            }
        }


        if (Input.GetAxisRaw("Horizontal") == -1 && !isMooving && playerAlreadyInteract)
        {          
            if(index > 0)
            {
                isMooving = true;
                moovingRight = true;
                index--;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") == 1 && !isMooving && playerAlreadyInteract)
        {           
            if(index < contenairs.Length -1)
            {
                isMooving = true;
                moovingLeft = true;
                index++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (moovingLeft)
            ContenairsMoovingLeft();
        else if (moovingRight)
            ContenairsMoovingRight();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
        }
    }

    private void ContenairsMoovingRight()
    {
        DialogOpen.instance.StartDialog(emptyDialog);
        canDisplayNextSentences = false;
        for (int i = 0; i < contenairs.Length -1; i++)
        {
            Vector3 currentPos = contenairs[i].transform.position;
            contenairs[i].transform.position = Vector3.MoveTowards(currentPos, startPosContenairs[i+1], speedDeplacement * Time.deltaTime);
        }

        contenairs[contenairs.Length - 1].transform.position = startPosContenairs[0];

        if (Vector3.Distance(contenairs[0].transform.position, startPosContenairs[1]) < 0.01f)  // check if one contenair is close enough to the target position
        {
            DialogOpen.instance.StartDialog(dialogs[index]);
            canDisplayNextSentences = true;
            UpdatePositionOrderAfterRightMoove();
            moovingRight = false;
            isMooving = false;
        }
    }

    private void ContenairsMoovingLeft()
    {
        DialogOpen.instance.StartDialog(emptyDialog);
        canDisplayNextSentences = false;
        for (int i = 1; i < contenairs.Length; i++)
        {
            Vector3 currentPos = contenairs[i].transform.position;
            contenairs[i].transform.position = Vector3.MoveTowards(currentPos, startPosContenairs[i - 1], speedDeplacement * Time.deltaTime);
        }

        contenairs[0].transform.position = startPosContenairs[contenairs.Length-1];

        if(Vector3.Distance(contenairs[1].transform.position, startPosContenairs[0]) < 0.01f)   // check if one contenair is close enough to the target position
        {
            DialogOpen.instance.StartDialog(dialogs[index]);
            canDisplayNextSentences = true;
            UpdatePositionOrderAfterLeftMoove();
            moovingLeft = false;
            isMooving = false;
        }
    }

    private void UpdatePositionOrderAfterRightMoove()   // shifts elements by +1
    {
        (contenairs[0], contenairs[1], contenairs[2], contenairs[3], contenairs[4]) = (contenairs[4], contenairs[0], contenairs[1], contenairs[2], contenairs[3]);
        for (int i = 0; i < startPosContenairs.Length; i++)
        {
            startPosContenairs[i] = contenairs[i].transform.position;
        }
    }

    private void UpdatePositionOrderAfterLeftMoove()   // shifts elements by -1
    {
        (contenairs[0], contenairs[1], contenairs[2], contenairs[3], contenairs[4]) = (contenairs[1], contenairs[2], contenairs[3], contenairs[4], contenairs[0]);
        for (int i = 0; i < startPosContenairs.Length; i++)
        {
            startPosContenairs[i] = contenairs[i].transform.position;
        }
    }
}
