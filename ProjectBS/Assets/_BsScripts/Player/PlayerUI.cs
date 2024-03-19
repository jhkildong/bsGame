using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private static PlayerUI _instance;
    public static PlayerUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerUI>();
                if (_instance == null) return null;
            }
            return _instance;
        }
    }
    public Slider mySlider;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeHP(float hp)
    {
        if (mySlider == null)
            return;
        mySlider.value = hp;
    }
}
