using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_StraightMove_", menuName = "Monster/NormalMonster/StraightMove", order = 2)]
public class StraightMoveMonsterData : GroupMonsterData
{
    protected override void OnEnable()
    {
        base.OnEnable();
        _type |= MonsterType.StraightMove;
    }

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<StraightMoveMonster>();
        clone.Init(this);

        return clone;
    }
}