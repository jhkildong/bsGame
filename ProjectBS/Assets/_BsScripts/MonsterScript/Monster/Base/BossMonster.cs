using Yeon;
using UnityEngine;

public abstract class BossMonster : Monster
{
    public BossMonsterData BossData => _data as BossMonsterData;
    // Update is called once per frame
    public override void Init(MonsterData data)
    {
        base.Init(data as BossMonsterData);
    }
}