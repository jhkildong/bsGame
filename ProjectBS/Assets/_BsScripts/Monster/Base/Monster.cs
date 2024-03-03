using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster
{
    public MonsterData Data { get; private set; }

    public Monster(MonsterData data) => Data = data;
}
