using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_Group_", menuName = "Monster/NormalMonster/Group", order = 1)]
public class GroupNormalMonsterData : NormalMonsterData
{
    public int Amount => _amount;

    [SerializeField] private int _amount;

    private void OnEnable()
    {
        _type = MonsterType.Group;
    }

    public override GameObject CreateClone()
    {
        GameObject clone = new GameObject(Name);
        clone.AddComponent<GroupNormalMonster>().Init(this);

        return clone;
    }
}