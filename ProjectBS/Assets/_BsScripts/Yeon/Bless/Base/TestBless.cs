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
            LevelAttribute.LevelUp(2);
        }
    }
}


