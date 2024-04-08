using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundMonster : NormalMonster
{
    public SurroundMonsterData SurroundData => _data as SurroundMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data);
        gameObject.layer = (int)BSLayers.SurroundMonster;
    }
}
