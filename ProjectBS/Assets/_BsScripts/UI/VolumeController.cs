using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering;

public class VolumeController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI message;

    // 조절할 오디오 믹서 그룹
    public AudioMixerGroup mixerGroup;
    // 볼륨 조정을 위한 AudioMixer
    public AudioMixer mixer;

    private float volume;

 
    void Start()
    {
        SetFunction_UI();
        bool curVolume = mixer.GetFloat(mixerGroup.name, out volume);
        if (curVolume)
        {
            slider.value = Mathf.InverseLerp(-40f, 0f, volume) * 100f;
        }
    }

    private void SetFunction_UI()
    {
        slider.onValueChanged.AddListener(Function_Slider);
       
    }

    private void Function_Slider(float _value)
    {
        int intValue = Mathf.RoundToInt(_value);
        message.text = intValue.ToString();
        float volumeDB = Mathf.Lerp(-40f, 0f, _value / 100f);
        mixer.SetFloat(mixerGroup.name, volumeDB);
    }
}
