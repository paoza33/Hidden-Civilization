using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraView : MonoBehaviour
{
    public Camera secondaryCamera;
    public bool isSetDefaultPosCamera;
    public bool isStatic;
    public bool ifInstantaneousMovement;
    public bool ifIgnoreTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isSetDefaultPosCamera)
                SetCameraView.instance.SetNewPosCamera(secondaryCamera.transform.position, secondaryCamera.transform.rotation, isStatic, ifInstantaneousMovement);
            else if (isSetDefaultPosCamera)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, CameraMovement.instance.CameraRotation, isStatic, ifInstantaneousMovement);
            }
            else
                Debug.Log("error triggerCameraView");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!ifIgnoreTriggerExit)
            {
                if (isSetDefaultPosCamera)
                {
                    SetCameraView.instance.SetNewPosCamera(secondaryCamera.transform.position, secondaryCamera.transform.rotation, isStatic, ifInstantaneousMovement);
                }
                else if (!isSetDefaultPosCamera)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, CameraMovement.instance.CameraRotation, false, ifInstantaneousMovement);
                }
                else
                    Debug.Log("error triggerCameraView");
            }
        }
    }
}