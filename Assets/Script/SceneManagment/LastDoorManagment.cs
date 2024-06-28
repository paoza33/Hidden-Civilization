using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LastDoorManagment : MonoBehaviour
{
    private List<int> orderSolution = new List<int>() {1, 4, 2, 3, 7, 5, 8, 6};
    private List<int> orderPlayer = new List<int> ();

    public GameObject secretDoor;
    public GameObject labyrinth;
    public GameObject triggerWarning;

    public GameObject[] Spheres;
    public GameObject[] triggerSpheres;

    public Transform newPosCamera;

    public bool solutionEditor = false;

    public GameObject playerStart;
    public Transform spwanLabyrinth;

    public AudioClip clip;

    public Dialog dialog;

    public static LastDoorManagment instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il existe plus d'une instance de LastDoorManagment.");
            return;
        }
        instance = this;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Labyrinth")
        {
            playerStart.transform.position = spwanLabyrinth.position;
            for (int i = 0; i < triggerSpheres.Length; i++)
            {
                triggerSpheres[i].gameObject.SetActive(false);
            }
            secretDoor.GetComponent<FlickeringEmissive>().enabled = true;
            labyrinth.GetComponent<FlickeringEmissive>().enabled = true;
            secretDoor.GetComponent<MeshCollider>().enabled = false;
        }
        DialogOpen.instance.StartDialog(dialog);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(Fade());
                //enabled = false; //a enlever à la fin
            }
        }
        if (solutionEditor)
        {
            for (int i = 0; i < triggerSpheres.Length; i++)
            {
                triggerSpheres[i].gameObject.SetActive(false);
            }
            StartCoroutine(CameraEffect());
            enabled = false;
        }
    }

    public void AddOrderPlayer(int symboleID)
    {
        if(orderPlayer.Count < orderSolution.Count) // pour eviter le cas d'erreur du depassement de tableau (qui ne devrait pas arriver)
        {
            orderPlayer.Add(symboleID);
        }
        if ((orderPlayer.Count == orderSolution.Count) && orderPlayer.SequenceEqual(orderSolution))   // si l'ordre du joueur est correct, on affiche la porte secrete
        {
            for(int i = 0; i< triggerSpheres.Length; i++)
            {
                triggerSpheres[i].gameObject.SetActive(false);
            }
            StartCoroutine(CameraEffect());
        }
        else if ((orderPlayer.Count == orderSolution.Count) && !orderPlayer.SequenceEqual(orderSolution))
        {
            for(int i=0;  i<Spheres.Length; i++)
            {
                Spheres[i].GetComponent<TriggerSphereLastDoor>().ResetInteraction();
            }
        }
    }

    private IEnumerator CameraEffect()
    {
        SetCameraView.instance.SetNewPosCamera(newPosCamera.position, newPosCamera.rotation, true, false);
        yield return new WaitForSeconds(2f);
        secretDoor.GetComponent<FlickeringEmissive>().enabled = true;
        labyrinth.GetComponent<FlickeringEmissive>().enabled = true;
        secretDoor.GetComponent<MeshCollider>().enabled = false;
        triggerWarning.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(2f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, CameraMovement.instance.CameraRotation, false, false);
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(clip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}
