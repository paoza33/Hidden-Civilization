using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il y a plus d'une instance de AudioManager");
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("tempAudio");
        tempGO.transform.position = pos;
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(tempGO, clip.length);
        return audioSource;
    }
}
