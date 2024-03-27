using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNormalMonster : NormalMonster
{
    public SingleNormalMonsterData SingleData => _data as SingleNormalMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data);
    }

}
