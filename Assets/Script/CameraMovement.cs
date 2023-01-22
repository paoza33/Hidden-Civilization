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
        if (!Input.GetButtonDown("Jump"))
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
    }

    public void SetIsOutdoor(bool _isOutdoor) {
        if (_isOutdoor)
        {
            posOffSet = new Vector3(0, 9, (float)-7.5);
            cameraRotation = Quaternion.Euler(50, 0, 0);
        }
        else
        {
            posOffSet = new Vector3(0, 7, (float)-3.5);
            cameraRotation = Quaternion.Euler(60, 0, 0);
        }
        transform.rotation = cameraRotation;
    }

    public void StartPosition(Transform _transform)
    {
        transform.position = _transform.position + posOffSet;
        newPosOffSetY = posOffSet.y;
    }
}