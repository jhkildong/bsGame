using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupNormalMonster : NormalMonster
{
    public GroupNormalMonsterData GroupData { get; private set; }

    public override void Init(MonsterData data)
    {
        base.Init(data);
        GroupData = (data as GroupNormalMonsterData);
    }

}
