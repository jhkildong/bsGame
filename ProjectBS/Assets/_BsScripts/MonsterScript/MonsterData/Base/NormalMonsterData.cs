using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalMonsterData : MonsterData
{
    public MonsterType Type => _type;
    public float Weight => _weight;
    [SerializeField] private float _weight;

    protected virtual void OnEnable()
    {
        
    }
    
    [SerializeField] protected MonsterType _type;
}
