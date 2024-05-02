using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public float Attack{get => _attack; set => _attack = value;}
    [SerializeField]private float _attack;


}
