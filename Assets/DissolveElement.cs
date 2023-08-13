using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DissolveElement : MonoBehaviour
{
    public float speed = 1;

    public static DissolveElement instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Dissolve(Material dissolveMat, string referenceVarShader)
    {
        StartCoroutine(Dissolution(dissolveMat, dissolveMat.GetFloat(referenceVarShader), referenceVarShader));
    }
    IEnumerator Dissolution(Material dissolveMat, float dissolveValue, string referenceVarShader)
    {
        if (!(dissolveValue > 1))
        {
            dissolveMat.SetFloat(referenceVarShader, dissolveValue);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Dissolution(dissolveMat, (dissolveMat.GetFloat(referenceVarShader) + 0.005f) * speed, referenceVarShader));
        }
    }

    public void Dissolve(Material dissolveMat, string referenceVarShader, GameObject gameObject)
    {
        StartCoroutine(Dissolution(dissolveMat, dissolveMat.GetFloat(referenceVarShader), referenceVarShader, gameObject));
    }
    IEnumerator Dissolution(Material dissolveMat, float dissolveValue, string referenceVarShader, GameObject gameObject)
    {
        if (!(dissolveValue > 1))
        {
            dissolveMat.SetFloat(referenceVarShader, dissolveValue);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Dissolution(dissolveMat, (dissolveMat.GetFloat(referenceVarShader) + 0.005f) * speed, referenceVarShader, gameObject));
        }
        else
        {
            Destroy(gameObject.gameObject);
        }
    }
    public void Dissolve(Material dissolveMat, string referenceVarShader, CubeObject cube)
    {
        StartCoroutine(Dissolution(dissolveMat, dissolveMat.GetFloat(referenceVarShader), referenceVarShader, cube));
    }
    IEnumerator Dissolution(Material dissolveMat, float dissolveValue, string referenceVarShader, CubeObject cube)
    {
        if(!(dissolveValue > 1))           
        {
            Debug.Log("test");
            dissolveMat.SetFloat(referenceVarShader, dissolveValue);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Dissolution(dissolveMat, (dissolveMat.GetFloat(referenceVarShader) + 0.005f) * speed, referenceVarShader, cube));
        }
        else
        {
            Debug.Log("destroy");
            Destroy(cube.gameObject);
        }
    }
}
