using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    public NormalMonsterData NormalData { get; private set; }

    public override void Init(MonsterData data)
    {
        Data = data;
        if(data is NormalMonsterData ndm)
        {
            NormalData = ndm;
            HP = MaxHP;
        }
    }
}
