using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagment : MonoBehaviour
{
    public new Transform camera;

    private bool ifLongFade;
    public Dialog dialog;

    private Animator animator;

    void Awake()
    {
        PlayerMovement.instance.StopMovement();
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        SaveDataSceneState saveDataSceneState = SaveDataManager.LoadDataSceneState();

        if (saveDataSceneState == null)
        {
            Debug.LogError("empty save file data scene state");
            return;
        }
        else if (saveDataSceneState != null && saveDataSceneState.towerState == 0)
        {
            ifLongFade = true;
            saveDataSceneState.towerState = 1;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
        }
        else if (saveDataSceneState != null && saveDataSceneState.towerState == 1)
        {
            ifLongFade = false;
        }
        if (ifLongFade)
        {
            DialogOpen.instance.StartDialog(dialog);
        }
        else
        {
            StartCoroutine(StartingFade());
        }
    }

    private void Start()
    {
        StartCoroutine(StartingFade());
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(StartingFade());
            }
        }
    }

    private IEnumerator StartingFade()
    {
        enabled = false;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}
