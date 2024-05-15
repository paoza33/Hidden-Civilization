using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractionChainSong : MonoBehaviour
{
    private bool playerAlreadyInteract = false;
    public AudioClip[] clip;
    public GameObject button;

    private GameObject player;

    public Transform cameraHigh;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)  // pour le moment, le joueur ne peut pas relancer l'audio tant qu'il n'est pas sortie du trigger
        {
            playerAlreadyInteract = true;
            StartCoroutine(PlayChainsSong(0));
            SetCameraView.instance.SetNewPosCamera(cameraHigh.position, cameraHigh.rotation, true, false);
            PlayerMovement.instance.StopMovement();
        }
        else if(Input.GetButtonDown("Escape") && playerAlreadyInteract)
        {
            StopAllCoroutines();
            PlayerMovement.instance.enabled = true;
            SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, Quaternion.Euler(60, 0, 0), false, false);
            playerAlreadyInteract = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = true;
            button.GetComponent<FlickeringEmissive>().isReverse = false;
            button.GetComponent<FlickeringEmissive>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
        button.GetComponent<FlickeringEmissive>().isReverse = true;
    }

    private IEnumerator PlayChainsSong(int index)
    {
        if(index < clip.Length)
        {
            AudioManager.instance.PlayClipAt(clip[index], transform.position);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(PlayChainsSong(index + 1));
        }
        else
        {
            PlayerMovement.instance.enabled = true;
            SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, Quaternion.Euler(60, 0, 0), false, false);  // modifier la camera pour qu'elle retourne a sa position/orientation initial
            playerAlreadyInteract = false;
        }
    }
}
