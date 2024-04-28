using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaAttackBuildingBase_ScriptableObject", menuName = "Building/AreaAttackBuildingData", order = 0)]
public class AreaAttackBuildingData : ScriptableObject
{
    public float atkPower => _atkPower;
    public float atkSpeed => _atkSpeed;
    public float hitDelay => _hitDelay;
    public float atkDuration => _atkDuration;
    public float atkRadius => _atkRadius;

    public LayerMask attackableLayer => _attackableLayer;


    [SerializeField] private float _atkPower;
    [SerializeField] private float _atkSpeed;  // 건물의 공격 주기
    [SerializeField] private float _hitDelay; // 건물공격의 타격 간격(장판형 공격의 경우)
    [SerializeField] private float _atkDuration; // 건물 공격의 지속시간 (장판형 공격의 경우)
    [SerializeField] private float _atkRadius; // 지점형 공격의 범위
    [SerializeField] private LayerMask _attackableLayer;
}
