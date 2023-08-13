using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserTowerManagment : MonoBehaviour
{
    public Transform laserOrigin;
    public float laserRange = 50f;

    LineRenderer laserLine;

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        laserLine.SetPosition(0, laserOrigin.position);
        laserLine.SetPosition(1, laserOrigin.position + laserOrigin.forward * laserRange);
    }
}
