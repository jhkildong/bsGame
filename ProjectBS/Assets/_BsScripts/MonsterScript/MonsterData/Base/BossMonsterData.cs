using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterData : MonsterData
{
    public MonsterType Type => _type;
    protected virtual void OnEnable()
    {

    }
    [SerializeField, ReadOnly] protected MonsterType _type;
    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<BossMonster>();
        clone.Init(this);

        return clone;
    }
}
