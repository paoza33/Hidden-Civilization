using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public Vector3 offsetPosition;
    public Vector3 cameraRotation;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
        CameraMovement.instance.StartPosition(offsetPosition, cameraRotation);
    }
}
