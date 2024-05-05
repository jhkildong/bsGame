using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI message;

    void Start()
    {
        SetFunction_UI();
    }

    private void SetFunction_UI()
    {
        slider.onValueChanged.AddListener(Function_Slider);
    }

    private void Function_Slider(float _value)
    {
        int intValue = Mathf.RoundToInt(_value);
        message.text = intValue.ToString();
    }
}
