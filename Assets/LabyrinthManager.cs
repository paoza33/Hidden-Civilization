using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthManager : MonoBehaviour
{

    public Queue<SubFracture> subFractureList = new Queue<SubFracture>();
    public Queue<Vector3> initialPosList = new Queue<Vector3>();
    public Queue<Quaternion> initialRotList = new Queue<Quaternion>();

    public static LabyrinthManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("il y a plus d'une instance de LabyrinthManagment");
            return;
        }
        instance = this;
        StartCoroutine(Fade());
    }

    public void ReconnectAllConnexion()
    {
        foreach (SubFracture subFracture in subFractureList)
        {
            subFracture.GetComponent<SubFracture>().enabled = false;
            subFracture.ReconnectAllConnexion();
            subFracture.transform.localPosition = initialPosList.Dequeue();
            subFracture.transform.rotation = initialRotList.Dequeue();
        }
        subFractureList.Clear();
        initialPosList.Clear();
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}