using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip chopSound;
    public AudioClip failSound; //  Added
    public AudioClip backgroundMusic;
    public AudioClip finishChopSound;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayChop()
    {
        if (chopSound != null)
            sfxSource.PlayOneShot(chopSound);
    }

    public void PlayFail() //  Added
    {
        if (failSound != null)
            sfxSource.PlayOneShot(failSound);
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayFinishChop()
    {
        if (finishChopSound != null)
            sfxSource.PlayOneShot(finishChopSound);
    }

}
