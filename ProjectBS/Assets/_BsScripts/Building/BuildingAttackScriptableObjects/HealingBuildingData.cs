using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_HealingBuilding", menuName = "Building/HealingBuildingData", order = 0)]
public class HealingBuildingData : ScriptableObject
{

    public float healAmount => _healAmount;
    public float healDelay => _healDelay;
    public bool hasDuration => _hasDuratuon;
    public float duration => _duration;
    public LayerMask targetLayer => _targetLayer;

 
    [SerializeField] private float _healAmount; // ����
    [SerializeField] private float _healDelay; // �� ƽ�� ������
    [SerializeField] private bool _hasDuratuon; // ���ӽð��� �ִ°�?
    [SerializeField] private float _duration; // �ǹ� ���ӽð�
    [SerializeField] private LayerMask _targetLayer; // ���� Ÿ�� ���̾�

}
