using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

[RequireComponent(typeof(MonsterFollowPlayer))]
[RequireComponent(typeof(MonsterAction))]
public class NormalMonster : Monster
{
    public NormalMonsterData NormalData { get; private set; }

    public override void Init(MonsterData data)
    {
        Data = data;
        if (data is NormalMonsterData normalData)
        {
            NormalData = normalData;
            CurHp = MaxHP;
            Instantiate(data.MonsterPrefab, this.transform); //�ڽ����� ������ ������ ����
            
        }
    }
}
