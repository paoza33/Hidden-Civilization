using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRules : MonoBehaviour
{
    public static SceneRules instance;
    public bool playerCanJump;
    public bool isOutdoor;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il y a plus d'une instance de SceneRules");
            return;
        }
        instance = this;
        PlayerMovement.instance.SetCanJump(playerCanJump);
        CameraMovement.instance.SetIsOutdoor(isOutdoor);
    }
}
