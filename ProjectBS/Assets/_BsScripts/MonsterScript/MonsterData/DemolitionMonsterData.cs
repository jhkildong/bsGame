using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_Demolition_", menuName = "Monster/NormalMonster/Demolition", order = 4)]
public class DemolitionMonsterData : GroupMonsterData
{
    public short BuildingDmg => _buildingDmg;
    public float AttackRange => _attackRange;

    [SerializeField] private short _buildingDmg;    //�ǹ� �߰� ������
    [SerializeField] private float _attackRange;    //���� ����

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<DemolitionMonster>();
        clone.Init(this);

        return clone;
    }
}
