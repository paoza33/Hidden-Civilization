using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraView : MonoBehaviour
{
    private Vector3 newPosition;
    private Quaternion newRotation;

    private bool ifInstantaneous;

    public static SetCameraView instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        enabled = false;
    }

    public void SetNewPosCamera(Vector3 _newPosition, Quaternion _newRotation, bool ifStatic, bool instantaneousMovement)
    {
        ifInstantaneous = instantaneousMovement;
        newPosition = _newPosition;
        newRotation = _newRotation;
        if (ifStatic) // if not static, camera follow Player
            CameraMovement.instance.enabled = false;
        else
            CameraMovement.instance.enabled = true;
        enabled = true;
    }

    private void MooveCamera()
    {
        if (ifInstantaneous)
        {
            Camera.main.transform.position = newPosition;
            Camera.main.transform.rotation = newRotation;
        }
        else
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPosition, 2f);
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, newRotation, 4f);
        }
        
        if (Vector3.Distance(Camera.main.transform.position, newPosition) < 0.1f && Quaternion.Angle(Camera.main.transform.rotation, newRotation) < 0.1f)
        {
            enabled = false;
        }
            
    }

    private void FixedUpdate()
    {
        MooveCamera();
    }
}