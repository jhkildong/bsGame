using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalMonsterData : MonsterData
{
    public bool BuildingFirst => _buildingFirst;
    public MonsterType Type => _type;
    
    [SerializeField] private bool _buildingFirst;
    [SerializeField, ReadOnly] protected MonsterType _type;
}
