using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalMonsterData : MonsterData
{
    public bool BuildingFirst => _buildingFirst;
    [SerializeField] private bool _buildingFirst;
}
