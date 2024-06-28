using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsSystem : MonoBehaviour
{
    public AudioMixer audioMixer;
    private Resolution[] resolutions;
    public TMP_Dropdown dropdown;

    public GameObject settings;
    public GameObject[] menuButtons;

    private void Start()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if ((resolutions[i].width == Screen.currentResolution.width) &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("Sound", volume);
    }

    public void SettingsDisplay()
    {
        foreach(GameObject obj in menuButtons)
        {
            obj.SetActive(false);
        }
        settings.SetActive(true);
    }

    public void SettingsBack()
    {
        bool isMenu = SceneManager.GetActiveScene().name == "Level0";

        settings.SetActive(false);
        foreach (GameObject obj in menuButtons)
        {
            if (obj.name == "Start")
                obj.SetActive(isMenu);

            else if (obj.name == "Resume")
                obj.SetActive(!isMenu);

            else
                obj.SetActive(true);
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
