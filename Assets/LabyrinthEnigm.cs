using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LabyrinthEnigm : MonoBehaviour // Le trigger de la sphere gere toute l'enigme du labyrinthe en un script (celui-la) contrairement au script "SphereLevitation" qui renvoi la gestion de l'engime a "PanelControl"
{

    public GameObject sphere;

    private bool canSetCameraLabyrinthe = true;

    public GameObject[] circles;
    private Quaternion[] targetRotations = new Quaternion[3];
    private int indexCircle = 0;    // value must be 0, 1 or 2;

    public float speed; // speed of rotation

    /*
     * direction of rotation of [circle1, circle2, circle3], value must be -1, 0 or 1;
     * 1 for clockwise,
     * -1 for the opposite
     * 0 for no rotation
    */
    private int[] directionRotations;

    private bool isMooving = false;

    private bool stopMoovingC1, stopMoovingC2, stopMoovingC3; // when all the boolean are false, stop calling the circles movement fonction

    public GameObject cameraLabyrinthe; // new camera position for labyrinthe enigme

    private void Awake()
    {
        enabled = false;
        for (int i = 0; i < circles.Length; i++)
        {
            targetRotations[i] = circles[i].transform.rotation;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // start the labyrinthe enigm
        if (Input.GetButtonDown("Interact") && !isMooving)
        {
            if (canSetCameraLabyrinthe) // set main camera position to the cameraLabyrinthe position
            {
                canSetCameraLabyrinthe = false;
                SetCameraView.instance.SetNewPosCamera(cameraLabyrinthe.transform.position, cameraLabyrinthe.transform.rotation, true, false);
                PlayerMovement.instance.StopMovement();
                circles[indexCircle].GetComponent<FlickeringEmissive>().isReverse = false;
            }
            else
            {
                if (indexCircle == 0)
                {
                    SetTargetRotation1();
                }
                else if (indexCircle == 1)
                {
                    SetTargetRotation2();
                }
                else if (indexCircle == 2)
                {
                    SetTargetRotation3();
                }
                else
                {
                    Debug.LogError("index circle must be between 0 and 2");
                }
                isMooving = true;
                stopMoovingC1 = false; stopMoovingC2 = false; stopMoovingC3 = false;
            }
        }

        // exit the labyrinthe enigm
        if (Input.GetButtonDown("Escape") && !isMooving)
        {
            circles[indexCircle].GetComponent<FlickeringEmissive>().isReverse = true;
            canSetCameraLabyrinthe = true;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, CameraMovement.instance.CameraRotation, false, false);
            PlayerMovement.instance.enabled = true;
            sphere.GetComponent<FlickeringEmissive>().isReverse = false;
        }

        // change the circle to moove
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") == 1 && !isMooving && !canSetCameraLabyrinthe) // !canSetCameraLabyrinthe because we don't want the circles[indexCircle] to be emissive and change the indexCircle value before interaction
                                                                                                                           // Input.GetButtonDown("Vertical") to prevent the indexCircle value to change to quickly
        {
            if (!(indexCircle == 2))
            {
                circles[indexCircle].GetComponent<FlickeringEmissive>().isReverse = true;
                indexCircle++;
                circles[indexCircle].GetComponent<FlickeringEmissive>().isReverse = false;
            }
        }
        else if(Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") == -1 && !isMooving && !canSetCameraLabyrinthe)
        {
            if(!(indexCircle == 0))
            {
                circles[indexCircle].GetComponent<FlickeringEmissive>().isReverse = true;
                indexCircle--;
                circles[indexCircle].GetComponent<FlickeringEmissive>().isReverse = false;
            }
        }


            // When circle 1 is near enough to the target rotation, we stop the rotation
            if (Quaternion.Angle(circles[0].transform.rotation, targetRotations[0]) < 0.1f && isMooving)
        {
            directionRotations[0] = 0;  //  stop the circle 1 rotation
            stopMoovingC1 = true;
        }
        if (Quaternion.Angle(circles[1].transform.rotation, targetRotations[1]) < 0.1f && isMooving)
        {
            stopMoovingC2 = true;
            directionRotations[1] = 0;  // stop the circle 2 rotation
        }
        if (Quaternion.Angle(circles[2].transform.rotation, targetRotations[2]) < 0.1f && isMooving)
        {
            stopMoovingC3 = true;
            directionRotations[2] = 0;  // stop the circle 3 rotation
        }
    }

    private void FixedUpdate()
    {
        if(isMooving)
        {
            TurnCircle();
        }
        if(stopMoovingC1 && stopMoovingC2 && stopMoovingC3)
        {
            isMooving = false;  // stop calling the function TurnCircle()
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enabled = true;
            sphere.GetComponent<FlickeringEmissive>().isReverse = false;
            sphere.GetComponent<FlickeringEmissive>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {            
            sphere.GetComponent<FlickeringEmissive>().isReverse = true;
            sphere.GetComponent<FlickeringEmissive>().enabled = true;
            enabled = false;
        }
    }

    private void TurnCircle()
    {
        circles[0].transform.rotation = Quaternion.Euler(circles[0].transform.rotation.eulerAngles + Vector3.up * directionRotations[0] * speed);
        circles[1].transform.rotation = Quaternion.Euler(circles[1].transform.rotation.eulerAngles + Vector3.up * directionRotations[1] * speed);
        circles[2].transform.rotation = Quaternion.Euler(circles[2].transform.rotation.eulerAngles + Vector3.up * directionRotations[2] * speed);
    }

    private void SetTargetRotation1()
    {
        directionRotations = new int [3] {1, -1, 0};
        targetRotations[0] = Quaternion.Euler(circles[0].transform.rotation.eulerAngles + new Vector3(0, 90f, 0));
        targetRotations[1] = Quaternion.Euler(circles[1].transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
    }

    private void SetTargetRotation2()
    {
        directionRotations = new int[3] {-1, 1, -1};
        targetRotations[0] = Quaternion.Euler(circles[0].transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
        targetRotations[1] = Quaternion.Euler(circles[1].transform.rotation.eulerAngles + new Vector3(0, 90f, 0));
        targetRotations[2] = Quaternion.Euler(circles[2].transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
    }

    private void SetTargetRotation3()
    {
        directionRotations = new int[3] {0, -1, 1};
        targetRotations[1] = Quaternion.Euler(circles[1].transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
        targetRotations[2] = Quaternion.Euler(circles[2].transform.rotation.eulerAngles + new Vector3(0, 90f, 0));
    }
}
