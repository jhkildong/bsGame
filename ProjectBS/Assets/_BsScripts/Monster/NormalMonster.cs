using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class NormalMonster : Monster
{
    public NormalMonsterData NormalData { get; private set; }

    protected override void Start()
    {
        Init(this.Data);
    }

    public override void Init(MonsterData data)
    {
        base.Init(data);
        NormalData = (data as NormalMonsterData);
    }
}
