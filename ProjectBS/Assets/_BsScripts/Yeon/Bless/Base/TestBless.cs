using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class TestBless : Yeon.Bless
{
    public float testValue { get => _testValue; set => _testValue = value; }

    [SerializeField]private float _testValue;

    // Start is called before the first frame update
    void Start()
    {
        //LevelProp.SetAct();
        LevelProp.LevelUp(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
