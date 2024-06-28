using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeathZoneLabyrinth : MonoBehaviour
{
    public List<SubFracture> subFractureList;

    public AudioClip audioClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlayClipAt(audioClip, other.transform.position);
            foreach (SubFracture subFracture in subFractureList)
            {
                LabyrinthManager.instance.subFractureList.Enqueue(subFracture);
                LabyrinthManager.instance.initialPosList.Enqueue(subFracture.transform.localPosition);
                LabyrinthManager.instance.initialRotList.Enqueue(subFracture.transform.rotation);
                subFracture.GetComponent<SubFracture>().enabled = true;
                subFracture.BreakAllConnexion();
            }
        }
    }
}
