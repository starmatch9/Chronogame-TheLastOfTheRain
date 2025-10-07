using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManger : MonoBehaviour
{
    public bool isPlaying = false;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayVoice(AudioClip clip)
    {
        if (clip == null) return;

        if (!isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
            isPlaying = true;
        }
    }
    public void StopVoice()
    {
        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
