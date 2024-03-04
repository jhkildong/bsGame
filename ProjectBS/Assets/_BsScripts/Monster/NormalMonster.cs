using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    public NormalMonsterData NormalData { get; private set; }


    public NormalMonster(NormalMonsterData data) : base(data)
    {
        NormalData = data;
    }

}
