using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_BuffBuilding", menuName = "Building/BuffBuildingData", order = 0)]
public class BuffBuildingData : ScriptableObject
{
    public float buffAmount => _buffAmount;
    public LayerMask targetLayer => _targetLayer;

    [SerializeField] private float _buffAmount;
    [SerializeField] private LayerMask _targetLayer;
}
