using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : FollowingTargetUI
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image sliderImage;
    private Color sliderColor;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        sliderImage = slider.fillRect.GetComponentInChildren<Image>();
        sliderColor = sliderImage.color;
    }

    private void OnEnable()
    {
        slider.value = 0;
    }

    public void ChnageProgress(float progress)
    {
        slider.value = progress;
        if(progress >= 1)
        {
            myTarget = null;
            UIManager.Instance.ReleaseUI(this);
        }
    }

    public void Selected(bool isSelected)
    {
        sliderColor.a = isSelected ? 1.0f : 0.5f;
        sliderImage.color = sliderColor;
    }
}