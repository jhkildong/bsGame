using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalMonsterData : MonsterData
{
    public MonsterType Type => _type;

    protected virtual void OnEnable()
    {
        
    }
    
    [SerializeField, ReadOnly] protected MonsterType _type;
}
