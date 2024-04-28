using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    public override int ID => _id;
    [SerializeField]private int _id = 5001;

    public Slider MySlider => _slider;
    [SerializeField]private Slider _slider;

    public TextMeshProUGUI myWoodText => _myWoodText;
    [SerializeField] private TextMeshProUGUI _myWoodText;
    public TextMeshProUGUI myStoneText => _myStoneText;
    [SerializeField] private TextMeshProUGUI _myStoneText;
    public TextMeshProUGUI myIronText => _myIronText;
    [SerializeField] private TextMeshProUGUI _myIronText;


    public void ChangeHP(float hp)
    {
        if (_slider == null)
            return;
        _slider.value = hp;
    }
}
