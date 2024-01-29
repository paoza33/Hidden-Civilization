using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagment : MonoBehaviour
{
    public Transform camera;
    void Awake()
    {
        SetCameraView.instance.SetNewPosCamera(camera.transform.position, camera.transform.rotation, true, true);
    }
}
