using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public static CubeMovement instance;
    int[] gridCoordinate;
    public Vector3 origin;
    public float step;
    private Vector3 currentPos;
    private Vector3 newWorldPos;
    private CubeObject cubeObject;
    private bool isCube = false;
    private float closeEnough = 0.01f;

    [SerializeField]
    private Shader shader;
    [SerializeField]
    private GameObject stele;


    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
            instance = this;
        gridCoordinate = new int[2];
        enabled = false;
    }

    public void Moove(CubeObject cube, string direction)
    {
        cubeObject = cube;
        SetNewPosition(cube, direction);
        enabled = true;
    }

    // set coordonnées [x,y]
    private void SetNewPosition(CubeObject mainCube, string direction)
    {
        if(direction == "Eastward")
        {
            for(int i = mainCube.positionY +1; i < 6; i++)
            {
                if (GridManagment.instance.gridScene[mainCube.positionX,i] != null) // on vérifie si il y a un cube sur le chemin
                {
                    gridCoordinate[0] = mainCube.positionX;
                    gridCoordinate[1] = i - 1; // si oui, notre position sera la position juste derriere le cube rencontre
                    isCube = true;
                    break;
                }
            }
            if (!isCube)
            {
                gridCoordinate[0] = mainCube.positionX; // si non, on le met hors de notre grid
                gridCoordinate[1] = 6;
            }
        }
        else if (direction == "Westward")
        {
            for (int i = mainCube.positionY - 1; i > 0; i--)
            {
                if (GridManagment.instance.gridScene[mainCube.positionX, i] != null)
                {
                    gridCoordinate[0] = mainCube.positionX;
                    gridCoordinate[1] = i + 1;
                    isCube = true;
                    break;
                }
            }
            if (!isCube)
            {
                gridCoordinate[0] = mainCube.positionX;
                gridCoordinate[1] = 0;
            }
        }
        else if (direction == "Northward")
        {
            for (int i = mainCube.positionX + 1; i < 6; i++)
            {
                if (GridManagment.instance.gridScene[i, mainCube.positionY] != null)
                {
                    gridCoordinate[0] = i - 1;
                    gridCoordinate[1] = mainCube.positionY;
                    isCube = true;
                    break;
                }
            }
            if (!isCube)    // si on ne rencontre pas de cube
            {
                gridCoordinate[0] = 6;
                gridCoordinate[1] = mainCube.positionY;
            }
        }
        else if (direction == "Southward")
        {
            for (int i = mainCube.positionX - 1; i > 0; i--)
            {
                if (GridManagment.instance.gridScene[i, mainCube.positionY] != null)
                {
                    gridCoordinate[0] = i + 1;
                    gridCoordinate[1] = mainCube.positionY;
                    isCube = true;
                    break;
                }
            }
            if (!isCube)
            {
                gridCoordinate[0] = 0;
                gridCoordinate[1] = mainCube.positionY;
            }
        }
        GridManagment.instance.gridScene[mainCube.positionX, mainCube.positionY] = null; // update grid
        GridManagment.instance.gridScene[gridCoordinate[0], gridCoordinate[1]] = mainCube;

        newWorldPos = new Vector3(origin.x + (step * gridCoordinate[1]), origin.y, origin.z + (step * gridCoordinate[0]));
        mainCube.positionX = gridCoordinate[0];
        mainCube.positionY = gridCoordinate[1];
    }


    private void FixedUpdate()
    {
        currentPos = cubeObject.transform.position;
        cubeObject.transform.position = Vector3.MoveTowards(currentPos, newWorldPos, 5f * Time.deltaTime);
        if (Vector3.Distance(cubeObject.transform.position, newWorldPos) < closeEnough) // les vecteurs ne sont pas egaux à des milliemes pres
        {
            if (!isCube)
            {
                cubeObject.GetComponent<Rigidbody>().isKinematic = false;
                cubeObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                isCube = false;
                if(cubeObject.GetComponent<CubeObject>().name == "MainCube")
                {
                    if((cubeObject.GetComponent<CubeObject>().positionX == 3) && (cubeObject.GetComponent<CubeObject>().positionY == 3))
                    {
                        stele.gameObject.SetActive(true);
                        cubeObject.GetComponent<MeshRenderer>().material.shader = shader;
                        DissolveElement.instance.Dissolve(cubeObject.GetComponent<MeshRenderer>().material, "_Dissolve_value", cubeObject);
                    }
                }
            }
            enabled = false;
        }
    }
}
