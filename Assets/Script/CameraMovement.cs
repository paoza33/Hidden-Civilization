using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 velocity;
    private float timeOffSet = 0.2f;
    private Vector3 posOffSet;
    private float newPosOffSetY;
    private Quaternion cameraRotation;
    public static CameraMovement instance;
    public bool isOutdoor, cameraFixX, cameraFixZ;
    public float fixX, fixZ;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il existe plus d'une instance de CameraMovement");
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (PlayerMovement.instance.GetisGrounded())
        {
            // Fix the camera position x/z if the player raise the limit of the level

            if(!cameraFixX && !cameraFixZ)
            {
                transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffSet, ref velocity, timeOffSet);
            }
            else if(cameraFixX && cameraFixZ)
            {
                Vector3 vFix = new Vector3(fixX, player.transform.position.y, fixZ);
                transform.position = Vector3.SmoothDamp(transform.position, vFix + posOffSet, ref velocity, timeOffSet);
            }
            else if (cameraFixX)
            {
                Vector3 vFixX = new Vector3(fixX, player.transform.position.y, player.transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, vFixX + posOffSet, ref velocity, timeOffSet);
            }
            else
            {
                Vector3 vFixZ = new Vector3(player.transform.position.x, player.transform.position.y, fixZ);
                transform.position = Vector3.SmoothDamp(transform.position, vFixZ + posOffSet, ref velocity, timeOffSet);
            }
            // Axis Y updated for when player was falling
            newPosOffSetY = player.transform.position.y + posOffSet.y;
        }
        else
        {
            // Fix axis Y when player fall

            Vector3 posTarget = new Vector3(player.transform.position.x + posOffSet.x, newPosOffSetY, player.transform.position.z + posOffSet.z);
            transform.position = Vector3.SmoothDamp(transform.position, posTarget, ref velocity, timeOffSet);
        }
    }

    public void StartPosition(Vector3 _posOffset, Vector3 newRotation)
    {
        posOffSet = _posOffset;
        transform.position = player.transform.position + posOffSet;
        newPosOffSetY = posOffSet.y;
        transform.rotation = Quaternion.Euler(newRotation);
    }

    public void StartPosition(Vector3 _posOffset, Quaternion newRotation)
    {
        posOffSet = _posOffset;
        cameraRotation = newRotation;
        transform.position = player.transform.position + posOffSet;
        newPosOffSetY = posOffSet.y;
        transform.rotation = newRotation;
    }

    public Vector3 PosOffSet
    {
        get { return posOffSet; }
    }

    public Quaternion CameraRotation
    {
        get { return cameraRotation; }
    }
}