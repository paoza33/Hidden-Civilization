using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    private GameObject menuUI, verifUI;

    private bool gameIsPaused;
    public GameObject start;
    public GameObject resume;
    public string levelToLoad; // temporaire pour gain de temps

    private void Awake()
    {
        menuUI = GameObject.FindGameObjectWithTag("Menu");
        verifUI = GameObject.FindGameObjectWithTag("VerificationChoiceUI");

        menuUI.SetActive(false);
        verifUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Menu") && !(SceneManager.GetActiveScene().name == "Level0")
            && GameObject.FindGameObjectWithTag("UIMapText").GetComponent<TextMeshProUGUI>().enabled == false)
        {
            if (gameIsPaused)
            {
                menuUI.SetActive(false);
                PlayerMovement.instance.enabled = true;
                Time.timeScale = 1;
                gameIsPaused = false;
            }
            else
            {
                menuUI.SetActive(true);
                PlayerMovement.instance.StopMovement();
                Time.timeScale = 0;
                gameIsPaused = true;
            }
        }
    }

    public void verificationChoiceUI()
    {
        menuUI.SetActive(false);
        verifUI.SetActive(true);
    }

    public void VerificationNo()
    {
        verifUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void NewGame()
    {
        SaveDataManager.DeleteAllData();
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(FadeStartingGame());
    }

    public void Resume()
    {
        menuUI.SetActive(false);
        PlayerMovement.instance.enabled = true;
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void Menu()
    {
        StartCoroutine(FadeMenu());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeMenu()
    {
        enabled = false;
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene("Level0");
        enabled = true;
    }

    private IEnumerator FadeStartingGame()
    {
        enabled = false;
        menuUI.SetActive(false);
        verifUI.SetActive(false);
        start.SetActive(false);
        resume.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        SceneSpawn();
        enabled = true;
    }

    private void SceneSpawn()
    {
        if (!SaveDataManager.SpawnDataCreated())
        {
            SaveDataSpawn dataSpawn = new SaveDataSpawn()
            {
                currentSceneName = "Home",
                previousSceneName = "",

                spawnVillage = "",
                spawnWood = "",
                spawnCamp = "",
                spawnCity = "",
                spawnRuins = "",
                spawnLastDoor = "",
                spawnLibrary = "",
                spawnLostIsland = ""
            };
            SaveDataManager.SaveDataSpawn(dataSpawn);
        }
        if (!SaveDataManager.SceneStateDataCreated())
        {
            SaveDataSceneState data = new SaveDataSceneState
            {
                homeState = 0,
                woodState = 0,
                libraryState = 0,
                cityState = 0,
                ruinsState = 0,
                villageState = 0,
                towerState = 0,
                campState = 0,
                woodenHutState = 0
            };
            SaveDataManager.SaveDataSceneState(data);
        }
        SaveDataSpawn newData = SaveDataManager.LoadDataSpawn();
        string levelToLoad = newData.currentSceneName;
        AudioManager.instance.StopCurrentSong();

        SceneManager.LoadScene(levelToLoad);
    }
}
