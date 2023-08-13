using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractionChainSong : MonoBehaviour
{
    private bool playerAlreadyInteract = false;
    public AudioClip[] clip;
    public GameObject button;

    private void Awake()
    {
        enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && !playerAlreadyInteract)  // pour le moment, le joueur ne peut pas relancer l'audio tant qu'il n'est pas sortie du trigger
        {
            playerAlreadyInteract = true;
            StartCoroutine(PlayChainsSong(0));
        }
        else if(Input.GetButtonDown("Interact") && playerAlreadyInteract)
        {
            StopAllCoroutines();
            StartCoroutine(PlayChainsSong(0));
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
        if (other.CompareTag("Player"))
        {
            enabled = false;
            button.GetComponent<FlickeringEmissive>().isReverse = true;
            playerAlreadyInteract = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator PlayChainsSong(int index)
    {
        if(index < clip.Length)
        {
            AudioManager.instance.PlayClipAt(clip[index], transform.position);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(PlayChainsSong(index + 1));
        }
    }
}
