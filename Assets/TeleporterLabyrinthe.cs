using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterLabyrinthe : MonoBehaviour
{
    public GameObject lastPlateform, teleporter;
    public string levelToLoad;

    private Animator animator;
    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Teleportation());
        }
    }

    private IEnumerator Teleportation()
    {
        PlayerMovement.instance.StopMovement();
        lastPlateform.GetComponent<FlickeringEmissive>().enabled = true;
        teleporter.GetComponent<FlickeringEmissive>().isPingPong = false;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.75f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
