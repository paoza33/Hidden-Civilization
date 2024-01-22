using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingDeathZone : MonoBehaviour
{
    public Transform respawn;
    public float timeRespawn;
    public TimeManager timeManager;
    private Animator animator;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SetPlayerRespawnPosition(other));
            timeManager.DoSlowmotionFixed(2f);
            StartCoroutine(Fade());
        }
    }

    private IEnumerator SetPlayerRespawnPosition(Collider player)
    {
        yield return new WaitForSecondsRealtime(timeRespawn);
        player.gameObject.transform.position = respawn.position;
        Quaternion cam = Camera.main.transform.rotation;
        CameraMovement.instance.StartPosition(CameraMovement.instance.PosOffSet, cam);
        if(SceneManager.GetActiveScene().name == "Labyrinth")
            LabyrinthManager.instance.ReconnectAllConnexion();
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        animator.SetTrigger("FadeIn");
    }
}
