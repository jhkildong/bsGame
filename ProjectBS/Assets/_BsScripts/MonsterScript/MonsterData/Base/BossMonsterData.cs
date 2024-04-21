using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BM_", menuName = "Monster/BossMonster", order = 0)]
public class BossMonsterData : MonsterData
{
    public float AttackRange => _attackRange;

    [SerializeField] private float _attackRange;    //공격 범위

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);

        Monster clone = go.AddComponent(BossMonsterList.Lists[ID]) as BossMonster;
        clone.Init(this);

        return clone;
    }
}
