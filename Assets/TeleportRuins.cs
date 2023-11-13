using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportRuins : MonoBehaviour
{
    public GameObject barrier;
    public Dialog dialogBarrier;
    public Light lt;
    public string levelToLoad;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        // couleur light
        //lt.color -= (Color.white / 2.0f) * Time.deltaTime;
        lt.color = Color.red;
        Debug.Log(lt.color);
        if(Input.GetButtonDown("Interact")){
            if(!DialogOpen.instance.DisplayNextSentences()){
                PlayerMovement.instance.StopMovement();
                StartCoroutine(Fade());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            barrier.SetActive(true);
            DialogOpen.instance.StartDialog(dialogBarrier);
            PlayerMovement.instance.enabled = true;
            GetComponent<BoxCollider>().enabled = false;
            enabled = true;
        }
        
    }

    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.50f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        SceneManager.LoadScene(levelToLoad);
    }
}
