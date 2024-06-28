using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] plateformWhite, plateformRed, plateformBlue, plateformGreen, plateformTurquoise;

    public AudioClip audioClip;

    public static PlateformMovement instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe plus d'une instance de PlateformMovement.");
            return;
        }
        instance = this;
    }

    public void Moove(string color, int index)
    {
        switch (color)
        {
            case "White":
                plateformWhite[index].transform.rotation = Quaternion.Euler(plateformWhite[index].transform.rotation.eulerAngles + new Vector3(0, 90, 0));
                for (int i = 0; i < plateformWhite[index].transform.childCount; i++)
                {
                    Transform childTransform = plateformWhite[index].transform.GetChild(i);
                    if (childTransform.name.Contains("Cylinder"))
                    {
                        Debug.Log(childTransform.name);
                    }
                }
                break;
            case "Red":
                plateformRed[index].transform.rotation = Quaternion.Euler(plateformRed[index].transform.rotation.eulerAngles + new Vector3(0, 90, 0));
                for (int i = 0; i < plateformRed[index].transform.childCount; i++)
                {
                    Transform childTransform = plateformRed[index].transform.GetChild(i);
                    if (childTransform.name.Contains("Cylinder"))
                    {
                        Debug.Log(childTransform.name);
                    }
                }
                break;
            case "Blue":
                plateformBlue[index].transform.rotation = Quaternion.Euler(plateformBlue[index].transform.rotation.eulerAngles + new Vector3(0, 90, 0));
                break;
            case "Green":
                plateformGreen[index].transform.rotation = Quaternion.Euler(plateformGreen[index].transform.rotation.eulerAngles + new Vector3(0, 90, 0));
                break;
            case "Turquoise":
                plateformTurquoise[index].transform.rotation = Quaternion.Euler(plateformTurquoise[index].transform.rotation.eulerAngles + new Vector3(0, 90, 0));
                break;
            default:
                Debug.Log("error color in Moove fonction in class PlaterformMovement");
                break;
        }
        AudioManager.instance.PlayClipAt(audioClip, transform.position);
    }
}
