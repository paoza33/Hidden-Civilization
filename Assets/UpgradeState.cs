using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeState : MonoBehaviour
{
    public string[] scenesToUpgrade;
    public int[] statement; // sceneTuUpgrade[i] = statement[i]
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
                   UpgradingState(scenesToUpgrade[i], statement[i]);
                }
                SaveDataManager.SaveDataSceneState(state);
            }
            else
                enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!noInteraction)
            {
                enabled = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !alreadyInteract)
        {
            alreadyInteract = true;
            for(int i = 0; i < scenesToUpgrade.Length; i++)
            {
                UpgradingState(scenesToUpgrade[i], statement[i]);
            }
            SaveDataManager.SaveDataSceneState(state);
            enabled = false;
        }
    }

    private void UpgradingState(string scene, int _state)
    {
        switch(scene)
        {
            case "City":
                state.cityState = _state;
                break;

            case "Home":
                state.homeState = _state;
                break;

            case "Wood":
                state.woodState = _state;
                break;

            case "Library":
                state.libraryState = _state;
                break;

            case "Ruins":
                state.ruinsState = _state;
                break;

            case "Village":
                state.villageState = _state;
                break;

            case "Camp":
                state.campState = _state;
                break;

            case "WoodenHut":
                state.woodenHutState = _state;
                break;
        }
    }
}