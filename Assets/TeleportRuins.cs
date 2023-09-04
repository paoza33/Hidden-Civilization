using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportRuins : MonoBehaviour
{
    public GameObject barrier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            barrier.SetActive(true);
            StartCoroutine(ChangeScene());
        }
        
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Ruins");
    }
}
