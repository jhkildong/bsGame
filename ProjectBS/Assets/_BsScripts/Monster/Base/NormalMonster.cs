using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public abstract class NormalMonster : Monster
{
    public NormalMonsterData NormalData { get; private set; }

    public override void Init(MonsterData data)
    {
        base.Init(data);
        NormalData = (data as NormalMonsterData);
        if(NormalData.BuildingFirst)
        {
            GameObject AIPerception = new GameObject("AIPerception");
            AIPerception.transform.parent = transform;
            AIPerception.AddComponent<AIPerception>().Init((int)BSLayerMasks.Building,
                (building) => ChangeTarget(building), () => ResetTarget());
        }
    }
}
