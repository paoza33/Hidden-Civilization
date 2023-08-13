using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeathZoneLabyrinth : MonoBehaviour
{
    public List<SubFracture> subFractureList;
    private void OnTriggerEnter(Collider other)
    {
        foreach(SubFracture subFracture in subFractureList)
        {
            subFracture.BreakAllConnexion();
        }
    }
}
