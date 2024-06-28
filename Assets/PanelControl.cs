using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PanelControl : MonoBehaviour
{
    public bool isLvl1 = true, isLvl2, isLvl3;
    [SerializeField]
    private GameObject towerToRotate, towerTop;
    private bool IsMooving = false;
    private bool isEnding = false;
    [SerializeField]
    private float speed = 1f;

    private int direction = 1; // si la direction est dans le sens horaire, direction = 1, dans le cas contraire -1

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    [SerializeField]
    private int objectifRotationYLvl1, objectifRotationYLvl2, objectifRotationYLvl3;

    private bool clockwise = true;

    [SerializeField]
    private GameObject [] spheres;

    private int step;
    [SerializeField]
    private int[] panelControlStepLvl1, panelControlStepLvl2, panelControlStepLvl3;  // contient le nombre de step de 45°
    [SerializeField]
    private GameObject objectifLvl1, objectifLvl2, objectifLvl3;
    [SerializeField]
    private GameObject firstBand, secondBand, thirdBand;

    public int id;
    
    [SerializeField]
    private int idStatic;
    [SerializeField]
    private GameObject[] triggerSpheres;

    public GameObject passerelles;
    public GameObject Barier;
    [SerializeField]
    private bool lvlAchieved = false;
    private Vector3 offset;
    private Vector3 newPosTower;
    private Vector3 newPosTopTower;

    public AudioClip audioTowerRotation;
    public AudioClip audioTowerGoDown;

    public static PanelControl instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !IsMooving && !isEnding)  // if true, rotate tower
        {
            AudioManager.instance.PlayClipAt(audioTowerRotation, transform.position);
            initialRotation = towerToRotate.transform.rotation;
            if (isLvl1)
            {
                TowerMovementLvl1();    // rotation lvl1
            }  
            else if (isLvl2)
            {
                TowerMovementLvl2();    // rotation lvl2
            }   
            else if (isLvl3)
            {
                TowerMovementLvl3();    // rotation lvl3
            }

            IsMooving = true;
        }
        if (Quaternion.Angle(towerToRotate.transform.rotation, targetRotation) < 0.5f)  // true when the tower rotation is near to the target rotation
        {
            IsMooving = false;
            ObjectifAchieved(); // this fonction checks if the tower rotation corresponds to the objectif rotation
        }
        if(Input.GetKeyDown(KeyCode.Escape) && !IsMooving && !isEnding)  // the player can exit the panel control with "escape" button only if the laser don't moove
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, CameraMovement.instance.CameraRotation, false, false);
            PlayerMovement.instance.enabled = true;
            triggerSpheres[idStatic].GetComponent<SphereLevitation>().enabled = true;
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (IsMooving)
        {
            towerToRotate.transform.rotation = Quaternion.Euler(towerToRotate.transform.rotation.eulerAngles + Vector3.up * direction * speed); // rotate the tower
        }
        if (lvlAchieved)
        {
            towerToRotate.transform.position = Vector3.MoveTowards(towerToRotate.transform.position, newPosTower, 1f);
            towerTop.transform.position = Vector3.MoveTowards(towerTop.transform.position, newPosTopTower, 1f);
            if((Vector3.Distance(towerToRotate.transform.position, newPosTower) < 0.1f) && (Vector3.Distance(towerTop.transform.position, newPosTopTower) < 0.1f))
            {
                if (!isLvl3 && !isLvl1 && !isLvl2)
                {
                    AppearElement.instance.Appear(passerelles.GetComponent<MeshRenderer>().material, "_Appear_value");
                    DissolveElement.instance.Dissolve(Barier.GetComponent<MeshRenderer>().material, "_Dissolve_value", Barier);
                    DissolveElement.instance.Dissolve(towerTop.GetComponent<MeshRenderer>().material, "_Dissolve_value", towerTop);
                    towerToRotate.GetComponent<LineRenderer>().enabled = false;
                    StartCoroutine(CancelAllPanelsControl());
                }
                lvlAchieved = false;
            }
        }
    }

    public void TowerMovementLvl1() // rotate the tower in clockwise direction
    {
        direction = 1;
        step = panelControlStepLvl1[id];
        targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, 45f * step, 0));
    }

    public void TowerMovementLvl2() // rotate the tower in clockwise direction and the next time, rotate in the opposite direction
    {
        step = panelControlStepLvl2[id];
        if (clockwise)
        {
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, 45f * step, 0));
            direction = 1;
            clockwise = false;
        }
        else
        {
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, -45f * step, 0));
            direction = -1;
            clockwise = true;
        }
    }

    private void TowerMovementLvl3() // same as the lvl2, but swap the panels by the next one in a clockwise direction
    {
        step = panelControlStepLvl3[id];
        if (clockwise)
        {
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, 45f * step, 0));
            direction = 1;
            clockwise = false;
        }
        else
        {
            targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, -45f * step, 0));
            direction = -1;
            clockwise = true;
        }
        for (int i = 0; i < spheres.Length; i++)
        {
            spheres[i].GetComponent<PanelControl>().id = (spheres[i].GetComponent<PanelControl>().id + 1) % 4;
        }
    }

    private void ObjectifAchieved() // player has finished the lvl and brings the tower down a notch
    {
        if (isLvl1)
        {
            if (Mathf.Abs(towerToRotate.transform.rotation.eulerAngles.y - objectifRotationYLvl1) < 1f) // the player achieved the lvl when the rotation of the tower is near to the objectif rotation
            {
                AudioManager.instance.PlayClipAt(audioTowerGoDown, transform.position);
                offset = new Vector3(0, -6.71f, 0);
                newPosTower = towerToRotate.transform.position + offset;
                newPosTopTower = towerTop.transform.position + offset;
                lvlAchieved = true;
                for (int i = 0; i < spheres.Length; i++)
                {
                    spheres[i].GetComponent<PanelControl>().isLvl1 = false;
                    spheres[i].GetComponent<PanelControl>().isLvl2 = true;
                }
                objectifLvl1.GetComponent<FlickeringEmissive>().enabled = true; // true because active the reverse emmissive, at the end of reversing, FlickeringEmissive script's will be enable = false;
                objectifLvl2.GetComponent<FlickeringEmissive>().enabled = true; // true active the emissive
                firstBand.GetComponent<FlickeringEmissive>().enabled = true;
                secondBand.GetComponent<FlickeringEmissive>().enabled = true;
            }
        }
        else if (isLvl2)
        {
            if (Mathf.Abs(towerToRotate.transform.rotation.eulerAngles.y - objectifRotationYLvl2) < 1f)
            {
                AudioManager.instance.PlayClipAt(audioTowerGoDown, transform.position);
                offset = new Vector3(0, -6.71f, 0);
                newPosTower = towerToRotate.transform.position + offset;
                newPosTopTower = towerTop.transform.position + offset;
                lvlAchieved = true;
                for (int i = 0; i < spheres.Length; i++)
                {
                    spheres[i].GetComponent<PanelControl>().isLvl2 = false;
                    spheres[i].GetComponent<PanelControl>().isLvl3 = true;
                }
                objectifLvl2.GetComponent<FlickeringEmissive>().enabled = true;
                objectifLvl3.GetComponent<FlickeringEmissive>().enabled = true;
                secondBand.GetComponent<FlickeringEmissive>().enabled = true;
                thirdBand.GetComponent<FlickeringEmissive>().enabled = true;
            }
        }
        else if (isLvl3)
        {
            if (Mathf.Abs(towerToRotate.transform.rotation.eulerAngles.y - objectifRotationYLvl3) < 1f)
            {
                AudioManager.instance.PlayClipAt(audioTowerGoDown, transform.position);
                offset = new Vector3(0, -6.6277f, 0);
                newPosTower = towerToRotate.transform.position + offset;
                newPosTopTower = towerTop.transform.position + offset;
                lvlAchieved = true;
                for (int i = 0; i < spheres.Length; i++)
                {
                    spheres[i].GetComponent<PanelControl>().isLvl3 = false;
                }
                objectifLvl3.GetComponent<FlickeringEmissive>().enabled = true;
                thirdBand.GetComponent<FlickeringEmissive>().enabled = true;
            }
        }
    }

    private IEnumerator CancelAllPanelsControl()
    {
        isEnding = true;
        yield return new WaitForSeconds(3f);
        foreach(GameObject triggersphere in triggerSpheres)
        {
            triggersphere.GetComponent<BoxCollider>().enabled = false;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SetCameraView.instance.SetNewPosCamera(player.transform.position + CameraMovement.instance.PosOffSet, CameraMovement.instance.CameraRotation, false, false);
        PlayerMovement.instance.enabled = true;
        triggerSpheres[idStatic].GetComponent<SphereLevitation>().enabled = true;
        enabled = false;
    }
}
