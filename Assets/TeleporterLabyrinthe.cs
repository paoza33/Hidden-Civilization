using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterLabyrinthe : MonoBehaviour
{
    public GameObject lastPlateform, teleporter;
    void Update()
    {
        
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
        SceneManager.LoadScene("Labyrinth");
    }
}
