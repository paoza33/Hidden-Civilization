using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public GameObject inventoryUI;
    public static bool gameIsPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }
    void Paused()
    {
        inventoryUI.SetActive(true);
        PlayerMovement.instance.enabled = false;
        Time.timeScale = 0;
        gameIsPaused = true;
    }
    public void Resume()
    {
        inventoryUI.SetActive(false);
        PlayerMovement.instance.enabled = true;
        Time.timeScale = 1;
        gameIsPaused = false;
    }
}
