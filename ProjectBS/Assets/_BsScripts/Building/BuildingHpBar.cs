using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHpBar : FollowingTargetUI
{
    public Slider MySlider => _slider;
    [SerializeField] private Slider _slider;


    public void ChangeHP(float hp)
    {
        if (_slider == null)
            return;
        _slider.value = hp;
        if(_slider.value <= 0) // 건물 파괴시 체력바 제거
        {
            Release();
        }
    }

    public void Release()
    {
        UIManager.Instance.ReleaseUI(this);
    }
}
