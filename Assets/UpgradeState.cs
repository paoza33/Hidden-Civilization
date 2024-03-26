using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeState : MonoBehaviour
{
    public string[] scenesToUpgrade;
    private bool alreadyInteract;

    private SaveDataSceneState state;

    public bool noInteraction;

    private void Awake()
    {
        enabled = false;
        state = SaveDataManager.LoadDataSceneState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(noInteraction){
                for(int i = 0; i < scenesToUpgrade.Length; i++)
                {
                   UpgradingState(scenesToUpgrade[i]);
                }
                SaveDataManager.SaveDataSceneState(state);
            }
            else
                enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !alreadyInteract)
        {
            alreadyInteract = true;
            for(int i = 0; i < scenesToUpgrade.Length; i++)
            {
                UpgradingState(scenesToUpgrade[i]);
            }
            SaveDataManager.SaveDataSceneState(state);
            enabled = false;
        }
    }

    private void UpgradingState(string scene)
    {
        switch(scene)
        {
            case "City":
                state.cityState += 1;
                break;

            case "Home":
                state.homeState += 1;
                break;

            case "Wood":
                state.woodState += 1;
                break;

            case "Library":
                state.libraryState += 1;
                break;

            case "Ruins":
                state.ruinsState += 1;
                break;

            case "Village":
                state.villageState += 1;
                break;

            case "Camp":
                state.campState += 1;
                break;

            case "WoodenHut":
                state.woodenHutState += 1;
                break;
        }
    }
}