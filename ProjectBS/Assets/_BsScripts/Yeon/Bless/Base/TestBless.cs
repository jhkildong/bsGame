using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    public class TestBless : Bless
    {
        public float TestInt { get => _testInt; set => _testInt = value; }
        [SerializeField]private float _testInt;

        public void Start()
        {
            Init(Data);
            StartCoroutine(test());
        }
        
        IEnumerator test()
        {
            float timer = 0.0f;
            while(true)
            {
                timer -= Time.deltaTime;
                if(timer <= 0.0f)
                {
                    LevelAttribute.LevelUp(2);
                    timer = 1.0f;
                }
                yield return null;
            }
            
        }
    }
}


