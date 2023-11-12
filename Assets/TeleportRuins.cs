using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportRuins : MonoBehaviour
{
    public GameObject barrier;
    public Dialog dialogBarrier;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact")){
                if(!DialogOpen.instance.DisplayNextSentences()){
                    StartCoroutine(ChangeScene());
                }
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            barrier.SetActive(true);
            DialogOpen.instance.StartDialog(dialogBarrier);
            // enlever le box collider
            enabled = true;
        }
        
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Ruins");
    }
}
