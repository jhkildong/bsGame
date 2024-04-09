using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_Demolition_", menuName = "Monster/NormalMonster/Demolition", order = 4)]
public class DemolitionMonsterData : GroupMonsterData
{
    public short BuildingDmg => _buildingDmg;

    [SerializeField] private short _buildingDmg;    //�ǹ� �߰� ������

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<DemolitionMonster>();
        clone.Init(this);

        return clone;
    }
}
