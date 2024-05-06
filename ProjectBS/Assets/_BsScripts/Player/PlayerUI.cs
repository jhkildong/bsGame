using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : UIComponent
{
    public Slider MySlider => _slider;
    [SerializeField]private Slider _slider;

    public TextMeshProUGUI myWoodText => _myWoodText;
    [SerializeField] private TextMeshProUGUI _myWoodText;
    public TextMeshProUGUI myStoneText => _myStoneText;
    [SerializeField] private TextMeshProUGUI _myStoneText;
    public TextMeshProUGUI myIronText => _myIronText;
    [SerializeField] private TextMeshProUGUI _myIronText;

    public TextMeshProUGUI myGoldText => _myGoldText;
    [SerializeField] private TextMeshProUGUI _myGoldText;
    public TextMeshProUGUI curSec => _curSec;
    [SerializeField] private TextMeshProUGUI _curSec;

    public TextMeshProUGUI curMin => _curMin;
    [SerializeField] private TextMeshProUGUI _curMin;


    public Slider myExp => _myExp;
    [SerializeField] private Slider _myExp;



    /*
    public Slider MyExpBar => _expBar;
    [SerializeField] private Slider _expBar;
    */

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
    public void ChangeGoldText(int gold)
    {
        if (_myGoldText == null)
            return;
        _myGoldText.text = gold.ToString();
    }


    public void ChangeExp(int exp)
    {
        if (_myExp == null)
            return;
        Debug.Log(exp);
        Debug.Log(GameManager.Instance.RequireExp);
        Debug.Log((float)(exp / GameManager.Instance.RequireExp));
        _myExp.value = ((float)exp / (float)GameManager.Instance.RequireExp);
        Debug.Log(_myExp.value);
    }

}
