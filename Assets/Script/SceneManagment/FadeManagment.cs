using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManagment : MonoBehaviour
{
    private bool ifLongFade;
    public Dialog dialog;

    private Animator animator;

    public bool unfixX;
    public bool unfixZ;

    private void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        SaveDataSceneState saveDataSceneState = SaveDataManager.LoadDataSceneState();

        if(saveDataSceneState == null)
        {
            Debug.LogError("empty save file data scene state");
            return;
        }
        else if(saveDataSceneState != null && saveDataSceneState.homeState == 0)
        {
            ifLongFade = true;
            saveDataSceneState.homeState = 1;
            SaveDataManager.SaveDataSceneState(saveDataSceneState);
        }
        else if(saveDataSceneState != null && saveDataSceneState.homeState == 1)
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

    // Update is called once per frame
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
        PlayerMovement.instance.StopMovement();
        enabled = false;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        if (unfixX)
            CameraMovement.instance.cameraFixX = false;
        if (unfixZ)
            CameraMovement.instance.cameraFixZ = false;
    }
}
