using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public GameObject finger;
    public float minTime = 2f;
    public float maxTime = 5f;
    public AudioSource hazardSound;

    public bool IsFingerPresent { get; private set; } = false;

    void Start()
    {
        finger.SetActive(false);
        Invoke("TriggerHazard", Random.Range(minTime, maxTime));
    }

    void TriggerHazard()
    {
        IsFingerPresent = true;
        finger.SetActive(true);
        hazardSound?.Play();
        Invoke("EndHazard", 1.5f);
    }

    void EndHazard()
    {
        IsFingerPresent = false;
        finger.SetActive(false);
        Invoke("TriggerHazard", Random.Range(minTime, maxTime));
    }
}