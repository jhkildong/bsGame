using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComponent : CharacterComponent
{
    public Transform[] AttackPoints => _attackPoints;
    [SerializeField]private Transform[] _attackPoints;
}
