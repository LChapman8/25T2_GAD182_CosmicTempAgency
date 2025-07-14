using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource chopSound;
    public AudioSource hazardSound;
    public AudioSource failSound;

    public void PlayChop() => chopSound?.Play();
    public void PlayHazard() => hazardSound?.Play();
    public void PlayFail() => failSound?.Play();
}
