using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMonster : NormalMonster
{
    public SingleMonsterData SingleData => _data as SingleMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data);
    }

}
