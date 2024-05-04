using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public void PlaySoundTrack(int num)
    {
        audioSources[0].Play();
    }

    public void PlayBGM()
    {
        //audioSource.PlayOneShot(audioClips[0]);
        audioSource.Play();
    }
    void Start()
    {
        //PlaySoundTrack(0);
    }
}
