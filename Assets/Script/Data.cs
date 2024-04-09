using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Runtime.Serialization;

[Serializable]
public class SaveDataSpawn
{
    public string currentSceneName;
    public string previousSceneName;

    public string spawnVillage;
    public string spawnWood;
    public string spawnCamp;
    public string spawnCity;
    public string spawnRuins;
    public string spawnLastDoor;
    public string spawnLibrary;
    public string spawnLostIsland;
}

[Serializable]
public class SaveDataSceneState
{
    public int homeState;
    public int woodState;
    public int libraryState;
    public int cityState;
    public int ruinsState;
    public int villageState;
    public int towerState;
    public int campState;
    public int woodenHutState;
}

public static class SaveDataManager
{
    #region Save and load spawn data
    public static void SaveDataSpawn(SaveDataSpawn data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SpawnData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveDataSpawn saveData = new SaveDataSpawn()
        {
            currentSceneName = data.currentSceneName,
            previousSceneName = data.previousSceneName,
            spawnCamp = data.spawnCamp,
            spawnCity = data.spawnCity,
            spawnLastDoor = data.spawnLastDoor,
            spawnLibrary = data.spawnLibrary,
            spawnLostIsland = data.spawnLostIsland,
            spawnRuins = data.spawnRuins,
            spawnVillage = data.spawnVillage
        };

        binaryFormatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveDataSpawn LoadDataSpawn()
    {
        string path = Application.persistentDataPath + "/SpawnData.data";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SaveDataSpawn data = binaryFormatter.Deserialize(stream) as SaveDataSpawn;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    #endregion

    #region Save and load Scene state data
    public static void SaveDataSceneState(SaveDataSceneState data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SceneStateData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveDataSceneState saveData = new SaveDataSceneState()
        {
            homeState = data.homeState,
            woodState = data.woodState,
            libraryState = data.libraryState,
            cityState = data.cityState,
            ruinsState = data.ruinsState,
            villageState = data.villageState,
            campState = data.campState,
            woodenHutState = data.woodenHutState
        };

        binaryFormatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveDataSceneState LoadDataSceneState()
    {
        string path = Application.persistentDataPath + "/SceneStateData.data";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveDataSceneState data = binaryFormatter.Deserialize(stream) as SaveDataSceneState;
            stream.Close();

            Debug.Log("homeState = " + data.homeState);
            Debug.Log("woodState = " + data.woodState);
            Debug.Log("libraryState = " + data.libraryState);
            Debug.Log("CityState = " + data.cityState);
            Debug.Log("ruinsState = " + data.ruinsState);
            Debug.Log("villageState = " + data.villageState);
            Debug.Log("campState = " + data.campState);
            Debug.Log("woodenHutState = " + data.woodenHutState);

            return data;

        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }
    #endregion

    public static bool SpawnDataCreated()
    {
        string path = Application.persistentDataPath + "/SpawnData.data";
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public static bool SceneStateDataCreated()
    {
        string path = Application.persistentDataPath + "/SceneStateData.data";
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }
}
