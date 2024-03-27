using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupNormalMonster : NormalMonster
{
    public GroupNormalMonsterData GroupData => _data as GroupNormalMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data);
    }

}
