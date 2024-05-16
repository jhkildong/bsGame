using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHpBar : UIComponent
{
    public Slider MySlider => _slider;
    [SerializeField] private Slider _slider;

    public Transform myTarget;
    public Vector3 currentPos;
    public float width;
    public float height;

    public void ChangeHP(float hp)
    {
        if (_slider == null)
            return;
        _slider.value = hp;
        if(_slider.value <= 0) // �ǹ� �ı��� ü�¹� ����
        {
            Release();
        }
    }

    public void Release()
    {
        UIManager.Instance.ReleaseUI(this);
    }

    void Update()
    {
        if (myTarget != null)
            currentPos = myTarget.position - new Vector3(0,0, (height*0.5f)-0.2f); // ü�¹� ������ġ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentPos);
        if (screenPos.z > 0.0f)
        {
            transform.position = screenPos;
        }
        else
        {
            transform.position = new Vector3(0, 100000, 0);
        }
    }
}
