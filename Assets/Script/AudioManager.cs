using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;

    public AudioMixerGroup soundEffectMixer;
    public AudioSource audioSource;

    private int musicIndex;

    public static AudioManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il y a plus d'une instance de AudioManager");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            PlayNextSong();
        }
    }

    public void PlayThemeSong(AudioClip audioClip)
    {
        if(!audioSource.isPlaying)
        {
            playlist[0] = audioClip;
            audioSource.clip = playlist[0];
            audioSource.Play();
        }
        
    }

    void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % playlist.Length;
        audioSource.clip = playlist[musicIndex];
        audioSource.Play();
    }

    public void StopCurrentSong()
    {
        audioSource.clip = null;
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("tempAudio");
        tempGO.transform.position = pos;
        AudioSource audioSourceTemp = tempGO.AddComponent<AudioSource>();
        audioSourceTemp.clip = clip;
        audioSourceTemp.outputAudioMixerGroup = soundEffectMixer;
        audioSourceTemp.Play();
        Destroy(tempGO, clip.length);
        return audioSourceTemp;
    }
}
