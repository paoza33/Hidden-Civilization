using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChangePiece : MonoBehaviour
{
    public Renderer previousCeiling, nextCeiling;
    public GameObject previousLight, nextLight;

    public BoxCollider currentTrigger, nextTrigger;

    private float alphaValue = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Material materialP = previousCeiling.material;
            Material materialN = nextCeiling.material;

            Color color = materialP.color;

            previousLight.gameObject.SetActive(false);
            nextLight.gameObject.SetActive(true);

            currentTrigger.enabled = false;
            nextTrigger.enabled = true;

            StartCoroutine(FadeAlpha(materialP, materialN, color));

        }
    }

    private IEnumerator FadeAlpha(Material _materialP, Material _materialN, Color _color)
    {
        _color.a = alphaValue;
        _materialN.color = _color;

        _color.a = 1f - alphaValue;
        _materialP.color = _color;

        yield return new WaitForSeconds(0.001f);

        if(alphaValue >= 0f)
        {
            Debug.Log(alphaValue);
            alphaValue -= 0.01f;
            StartCoroutine(FadeAlpha(_materialP, _materialN, _color));
        }
        else
        {
            alphaValue = 1f;
        }
    }
}