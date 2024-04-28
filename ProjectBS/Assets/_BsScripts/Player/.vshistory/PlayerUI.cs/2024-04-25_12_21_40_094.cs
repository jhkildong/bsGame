using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    public override int ID => _id;
    [SerializeField]private int _id = 5001;

    public Slider MySlider => _slider;
    [SerializeField]private Slider _slider;

    public void ChangeHP(float hp)
    {
        if (_slider == null)
            return;
        _slider.value = hp;
    }
}
