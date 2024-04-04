using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundMonster : NormalMonster
{
    public SurroundMonsterData SurroundData => _data as SurroundMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data);
        rBody.mass = 1000;
        if (col is CapsuleCollider capsule)
        {
            capsule.radius = 0.5f;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
