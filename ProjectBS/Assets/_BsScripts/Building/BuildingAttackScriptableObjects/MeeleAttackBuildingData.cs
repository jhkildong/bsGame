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
    [SerializeField] private float _hitDelay; // �ǹ������� Ÿ�� ����
    [SerializeField] private short _atkRadius; // ������ ������ ����
    [SerializeField] private LayerMask _attackableLayer;
}
