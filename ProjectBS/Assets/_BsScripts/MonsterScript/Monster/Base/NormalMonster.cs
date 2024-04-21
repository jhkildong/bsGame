using UnityEngine;
using Yeon;

public abstract class NormalMonster : Monster
{
    public NormalMonsterData NormalData => _data as NormalMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data as NormalMonsterData);
    }




}
