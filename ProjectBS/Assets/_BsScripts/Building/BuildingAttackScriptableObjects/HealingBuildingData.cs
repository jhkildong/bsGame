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

 
    [SerializeField] private float _healAmount; // 힐량
    [SerializeField] private float _healDelay; // 힐 틱당 딜레이
    [SerializeField] private bool _hasDuratuon; // 지속시간이 있는가?
    [SerializeField] private float _duration; // 건물 지속시간
    [SerializeField] private LayerMask _targetLayer; // 버프 타겟 레이어

}
