using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNormalMonster : NormalMonster
{
    public SingleNormalMonsterData SingleData { get; private set; }

    public override void Init(MonsterData data)
    {
        base.Init(data);
        SingleData = (data as SingleNormalMonsterData);
    }

}
