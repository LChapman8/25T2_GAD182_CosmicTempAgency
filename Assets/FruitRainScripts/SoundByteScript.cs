using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundByteScript : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip pointSoundClip;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointSoundClip()
    {
        Debug.Log("Play BasketFruitSound Clip");
        audioSource.clip = pointSoundClip;
        audioSource.Play();

    }
}
