using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeeleAttackBuildingBase_ScriptableObject", menuName = "Building/MeeleAttackBuildingScriptableObject", order = 0)]
public class MeeleAttackBuildingData : ScriptableObject
{
    public float atkPower => _atkPower;
    public float hitDelay => _hitDelay;
    public float atkRadius => _atkRadius;
    public LayerMask attackableLayer => _attackableLayer;


    [SerializeField] private float _atkPower;
    [SerializeField] private float _hitDelay; // 건물공격의 타격 간격
    [SerializeField] private short _atkRadius; // 지점형 공격의 범위
    [SerializeField] private LayerMask _attackableLayer;
}
