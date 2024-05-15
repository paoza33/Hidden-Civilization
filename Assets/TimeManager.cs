using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime; //    unscaledDeltaTime not change when timescale change
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);   // for the time do not increase more than realtime (1f = realtime)
        if (Time.timeScale == 1f)
            enabled = false;
    }

    public void DoSlowmotion()
    {
        enabled = true;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;    // baisse la valeur fixedDeltaTime lorsqu'on baisse le timeScale pour eviter un effet "Lag"
                                                        // explication : fixedUpdate fonctionne par un temps fixé (fixed timer)
                                                        // alors que Update fonctionne a chaque frame
    }

    // same slowmotion but the timeScale not increase during slowmotion
    public void DoSlowmotionFixed(float _slowdownLength)
    {
        slowdownLength = _slowdownLength;
        StartCoroutine(SlowmotionLength());
    }

    private IEnumerator SlowmotionLength()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSecondsRealtime(slowdownLength);
        enabled = true;
    }
}
