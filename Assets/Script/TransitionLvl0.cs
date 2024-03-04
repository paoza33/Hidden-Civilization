using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLvl0 : MonoBehaviour
{
    public string levelToLoad;
    // Start is called before the first frame update
    void Start()
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
        if(!SaveDataManager.SceneStateDataCreated())
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
        //levelToLoad = newData.currentSceneName;
        SceneManager.LoadScene(levelToLoad);
    }
}
