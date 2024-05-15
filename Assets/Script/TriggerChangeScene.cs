using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerChangeScene : MonoBehaviour
{
    public string levelToLoad;
    private Animator animator;
    private bool alreadyTriggered;
    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        alreadyTriggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyTriggered)
        {
            PlayerMovement.instance.StopMovement();
            alreadyTriggered = true;
            StartCoroutine(Fade());
        }
    }
    private IEnumerator Fade()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.75f);
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        data.currentSceneName = levelToLoad;
        data.previousSceneName = SceneManager.GetActiveScene().name;

        SaveDataManager.SaveDataSpawn(data);

        SceneManager.LoadScene(levelToLoad);
    }
}
