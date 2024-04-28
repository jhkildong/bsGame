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
    [SerializeField] private float _atkSpeed;  // �ǹ��� ���� �ֱ�
    [SerializeField] private float _hitDelay; // �ǹ������� Ÿ�� ����(������ ������ ���)
    [SerializeField] private float _atkDuration; // �ǹ� ������ ���ӽð� (������ ������ ���)
    [SerializeField] private float _atkRadius; // ������ ������ ����
    [SerializeField] private LayerMask _attackableLayer;
}
