using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMonster : NormalMonster
{
    public GroupMonsterData GroupData => _data as GroupMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data);
    }

}
