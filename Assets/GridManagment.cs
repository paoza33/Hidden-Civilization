using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagment : MonoBehaviour
{
    public CubeObject [,] gridScene;
    public Vector3 origin;
    public float step;
    public static GridManagment instance;
    public GameObject mainCube;
    public GameObject secondaryCube;

    /* 1 = Main cube
     * 2 = secondary cube
     * -1 = no cube */

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        gridScene = new CubeObject[7, 7];
    }

    void Start()
    {
        // gridScene[column, linea] -> [0,0] == left down of the gridScene
        InitializeGrid(1, 1, 4, "MainCube");
        InitializeGrid(2, 2, 1, "SecondaryCube");
        InitializeGrid(3, 3, 1, "SecondaryCube");
        InitializeGrid(4, 3, 5, "SecondaryCube");
        InitializeGrid(5, 5, 2, "SecondaryCube");
        InitializeGrid(6, 5, 5, "SecondaryCube");        
    }
    private void InitializeGrid(int id, int x, int y, string name)
    {
        Vector3 pos = new Vector3(origin.x + (step * y), origin.y, origin.z + (step * x));
        if (name == "MainCube")
            gridScene[x, y] = Instantiate(mainCube, pos, Quaternion.identity).GetComponent<CubeObject>();
        else if (name == "SecondaryCube")
            gridScene[x, y] = Instantiate(secondaryCube, pos, Quaternion.identity).GetComponent<CubeObject>();
        gridScene[x,y].id = id;
        gridScene[x,y].positionX = x;
        gridScene[x,y].positionY = y;
        gridScene[x,y].name = name;
    }
}
