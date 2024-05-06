using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using Unity.VisualScripting;
using UnityEngine;

public class SoundObject : MonoBehaviour , IPoolable
{
    public MonoBehaviour This => this;
    public int ID => _id;
    public AudioSource audioSource => _audioSource;
    public AudioClip soundClip => _soundClip;

    public IPoolable CreateClone()
    {
        GameObject clone = Instantiate(prefab);

        return clone.GetComponent<SoundObject>();
    }

    [SerializeField] private int _id;
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _soundClip;

    void OnEnable()
    {
        audioSource.clip = soundClip;
        float audioLength = soundClip.length;
        StartCoroutine(AudioLengthCheck(audioLength));
    }
    IEnumerator AudioLengthCheck(float length)
    {
        yield return new WaitForSeconds(length);
        SoundManager.Instance.ReleaseObject(gameObject, ID);
    }
}
