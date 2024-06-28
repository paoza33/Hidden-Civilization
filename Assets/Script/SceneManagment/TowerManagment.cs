using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagment : MonoBehaviour
{
    public new Transform camera;

    public Dialog dialog;

    private Animator animator;

    private bool beggining = true;

    public AudioClip clip;

    void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();

        DialogOpen.instance.StartDialog(dialog);

    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && beggining)
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                SetCameraView.instance.SetNewPosCamera(camera.position, camera.rotation, true, true);
                beggining = false;
                StartCoroutine(StartingFade());
            }
        }
    }

    private IEnumerator StartingFade()
    {
        
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.PlayThemeSong(clip);
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}
