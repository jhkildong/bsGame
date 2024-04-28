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

    public Slider MyExpBar => _expBar;
    [SerializeField] private Slider _expBar;


    public void ChangeHP(float hp)
    {
        if (_slider == null)
            return;
        _slider.value = hp;
    }

    public void ChangeWoodText(int wood)
    {
        if (_myWoodText == null)
            return;
        _myWoodText.text = wood.ToString();
    }
    public void ChangeStoneText(int stone)
    {
        if (_myStoneText == null)
            return;
        _myStoneText.text = stone.ToString();
    }
    public void ChangeIronText(int iron)
    {
        if (_myIronText == null)
            return;
        _myIronText.text = iron.ToString();
    }

    public void ChangeExpBar(float exp)
    {
        if (_expBar == null)
            return;
        _expBar.value = exp;
    }


}
