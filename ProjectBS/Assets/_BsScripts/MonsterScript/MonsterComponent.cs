using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent : CharacterComponent
{
    public Transform[] AttackPoints => _attackPoints;
    public RootMotion RootMotion => _rootMotion;
    [SerializeField]private Transform[] _attackPoints;
    [SerializeField] private RootMotion _rootMotion;
}
