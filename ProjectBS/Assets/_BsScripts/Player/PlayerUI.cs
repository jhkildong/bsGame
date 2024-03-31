using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    /// <summary>
    /// 싱글턴 인스턴스
    /// </summary>
    private static PlayerUI _instance;
    public static PlayerUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerUI>();
                if (_instance == null)
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/UI/PlayerUI"));
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            transform.SetParent(MyCavas.transform);
            RectTransform rt = transform as RectTransform;
            rt.anchorMin = new Vector2(1, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.anchoredPosition = new Vector2(-rt.sizeDelta.x * 0.5f, -rt.sizeDelta.y * 0.5f);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


    public Slider MySlider => _slider;
    public Transform MyCavas
    {
        get
        {
            if(_canvas == null)
            {
                _canvas = GameObject.Find("Canvas").transform;
            }
            return _canvas;
        }
    }

    [SerializeField]private Slider _slider;
    [SerializeField]private Transform _canvas;

    public void ChangeHP(float hp)
    {
        if (_slider == null)
            return;
        _slider.value = hp;
    }
}
