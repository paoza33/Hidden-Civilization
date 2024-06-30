using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsManagment : MonoBehaviour
{
    public GameObject playerStart;

    public Transform spawnTower;
    public GameObject[] objState0;
    public GameObject[] objState1;

    public GameObject bodyguardLeft, bodyguardRight;

    public Dialog dialog0, dialog1;
    public Dialog[] dialogsEN;

    public AudioClip clip;

    private bool isEnglish;
    private void Awake()
    {
        isEnglish = LocaleSelector.instance.IsEnglish();

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Tower")
            playerStart.transform.position = spawnTower.position;
        
        SaveDataSceneState state = SaveDataManager.LoadDataSceneState();
        if(state.ruinsState == 0){
            foreach(GameObject obj in objState0)
                obj.SetActive(true);
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogsEN[0]);
            else
                DialogOpen.instance.StartDialog(dialog0);
        }
        else if(state.ruinsState == 1){
            bodyguardLeft.transform.rotation = Quaternion.Euler(bodyguardLeft.transform.rotation.eulerAngles + new Vector3(0, -90f, 0));
            bodyguardRight.transform.rotation = Quaternion.Euler(bodyguardRight.transform.rotation.eulerAngles + new Vector3(0, 90f, 0));
            foreach (GameObject obj in objState1)
                obj.SetActive(true);
            if(isEnglish)
                DialogOpen.instance.StartDialog(dialogsEN[1]);
            else
                DialogOpen.instance.StartDialog(dialog1);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!DialogOpen.instance.DisplayNextSentences())
            {
                StartCoroutine(Fade());
            }
        }
    }

    private IEnumerator Fade()
    {
        enabled = false;
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
