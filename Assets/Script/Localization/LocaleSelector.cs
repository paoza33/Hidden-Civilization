using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject frenchButton, englishButton, textSelectionLanguage;

    private bool isActive = false;

    private string currentLanguage;
    public static LocaleSelector instance;

    private void Awake()
    {
        instance = this;
    }

    public void BackLanguage()
    {
        menuUI.SetActive(false);
        frenchButton.SetActive(true);
        englishButton.SetActive(true);
        textSelectionLanguage.SetActive(true);
    }

    public void ChangeLocale(int localeID)
    {
        if (isActive == true)
            return;
        StartCoroutine(SetLocale(localeID));
    }
    IEnumerator SetLocale(int _localeID)
    {
        isActive = true;
        yield return LocalizationSettings.InitializationOperation;  // check when the system is ready and become true when it is
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];

        Locale currentLocale = LocalizationSettings.SelectedLocale;
        currentLanguage = currentLocale.Identifier.Code;

        isActive = false;
        menuUI.SetActive(true);
        frenchButton.SetActive(false);
        englishButton.SetActive(false);
        textSelectionLanguage.SetActive(false);
    }

    public bool IsEnglish(){
        return (currentLanguage == "en");
    }
}
