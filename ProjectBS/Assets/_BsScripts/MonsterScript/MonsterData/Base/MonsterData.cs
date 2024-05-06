using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>리스폰 타입</summary>
[System.Flags]
public enum MonsterType
{
    Single = 0b_0000_0001,
    Group = 0b_0000_0010,
    Surround = 0b_0000_0100,
    StraightMove = 0b_0000_1000,
}
public abstract class MonsterData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public float Ak => _attack;
    public float AkDelay => _attackDelay;
    public float Sp => _speed;
    public float MaxHP => _maxHp;
    public float Exp => _exp;
    public float Gold => _gold;
    public float Mass => _mass;
    public float Radius => _radius;
    public GameObject Prefab => _prefab;
    
    public List<dropItem> DropItemList => _dropItemList;
    

    [SerializeField] private int _id;               // 몬스터 아이디
    [SerializeField] private string _name;          // 몬스터 이름
    [SerializeField] private float _attack;         // 몬스터 공격력
    [SerializeField] private float _attackDelay;    // 몬스터 공격딜레이
    [SerializeField] private float _speed;          // 몬스터 이동속도
    [SerializeField] private float _maxHp;          // 몬스터 최대체력
    [SerializeField] private float _exp;            // 몬스터 경험치
    [SerializeField] private float _gold;           // 몬스터 골드
    [SerializeField] private float _mass;           // 몬스터 무게
    [SerializeField] private float _radius;         // 몬스터 크기
    [SerializeField] private GameObject _prefab;    // 몬스터 프리팹
    [SerializeField] private List<dropItem> _dropItemList;

    /// <summary> 타입에 맞는 새로운 몬스터 생성 </summary>
    public abstract Monster CreateClone();

}
