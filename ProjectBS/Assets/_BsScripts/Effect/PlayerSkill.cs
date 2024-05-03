using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public float Attack{get => _attack; set => _attack = value;}
    [SerializeField]private float _attack;
    public float Size { get => _size; set => _size = value;}
    private float _size = 1.0f;


}
