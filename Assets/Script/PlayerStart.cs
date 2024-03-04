using System.Collections;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public Vector3 offsetPosition;
    public Vector3 cameraRotation;

    private void Awake()
    {
        StartCoroutine(DelayApparitionPlayer());
    }

    private IEnumerator DelayApparitionPlayer() // pour �viter que le joueur spawn avant le d�placement du playerStart
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        yield return new WaitForSeconds(0.1f);
        player.transform.position = transform.position;
        CameraMovement.instance.StartPosition(offsetPosition, cameraRotation);
    }
}
