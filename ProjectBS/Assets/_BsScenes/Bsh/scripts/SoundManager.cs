using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSources;

    public void PlaySoundTrack(int num)
    {
        audioSources[num].Play();
    }
}
