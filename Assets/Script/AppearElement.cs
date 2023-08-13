using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearElement : MonoBehaviour
{
    public float speed = 1;
    private bool isOver = false;
    public static AppearElement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public bool Appear(Material appearMat, string referenceVarShader)
    {
        StartCoroutine(appearance(appearMat, appearMat.GetFloat(referenceVarShader), referenceVarShader));
        return isOver;
    }
    IEnumerator appearance(Material appearMat, float appearValue, string referenceVarShader)
    {
        if (!(appearValue < -1.5f))
        {
            appearMat.SetFloat(referenceVarShader, appearValue);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(appearance(appearMat, (appearMat.GetFloat(referenceVarShader) - 0.005f) * speed, referenceVarShader));
        }
        else
            isOver = true;
    }
}
