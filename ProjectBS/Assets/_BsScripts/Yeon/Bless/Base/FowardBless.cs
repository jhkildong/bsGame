using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class FowardBless : Yeon.Bless
{
    public float Speed { get => _speed; set => _speed = value; }
    [SerializeField]private float _speed;
}
