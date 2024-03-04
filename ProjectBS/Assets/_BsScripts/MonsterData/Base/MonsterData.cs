using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public float Ak => _attack;
    public float AkDelay => _attackDelay;
    public float Sp => _speed;
    public short MaxHP => _maxHp;
    public GameObject MonsterPrefab => _prefab;

    [SerializeField] private int _id;               // 몬스터 아이디
    [SerializeField] private string _name;          // 몬스터 이름
    [SerializeField] private short _attack;         // 몬스터 공격력
    [SerializeField] private short _attackDelay;    // 몬스터 공격딜레이
    [SerializeField] private short _speed;          // 몬스터 이동속도
    [SerializeField] private short _maxHp;          // 몬스터 최대체력
    [SerializeField] private GameObject _prefab;    // 몬스터 프리팹

    /// <summary> 타입에 맞는 새로운 몬스터 생성 </summary>
    public abstract Monster CreateMonster();

}
