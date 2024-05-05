using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_Surround_", menuName = "Monster/NormalMonster/Surround", order = 3)]
public class SurroundMonsterData : NormalMonsterData
{
    public float SurroundRange => _surroundRange;
    [SerializeField] private float _surroundRange;

    protected override void OnEnable()
    {
        _type = MonsterType.Surround;
    }

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<SurroundMonster>();
        clone.Init(this);

        return clone;
    }
}
