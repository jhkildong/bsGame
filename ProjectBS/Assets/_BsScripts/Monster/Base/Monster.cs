using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Monster
{
    public MonsterData Data { get; private set; }

    /// <summary> 현재 체력 </summary>
    public short HP { get; protected set; }
    /// <summary> 최대체력</summary>
    public short MaxHP => Data.MaxHP;
    /// <summary> 현재 체력이 0이하가 되면 true  </summary>
    public bool IsDead => HP <= 0;
    public void TakeDamage(short damage) => HP -= damage;
    public void ReceiveHeal(short heal)
    {
        if (IsDead)
            return;
        HP += heal;
        Mathf.Clamp(HP, 0, MaxHP);
    }

    protected float _speedCeof = 1.0f; //이동속도 계수
    /// <summary>몬스터 이동속도, 값 대입시 계수로 적용</summary>
    public float Speed { get => Data.Sp * _speedCeof;  set => _speedCeof = value; }
    public void ResetSpeed() => _speedCeof = 1.0f;

    protected float _attackCeof = 1.0f; //공격력 계수
    /// <summary>몬스터 공격력, 값 대입시 계수로 적용</summary>
    public float Attack { get => Data.Ak * _attackCeof; set => _attackCeof = value; }
    public void ResetAttack() => _attackCeof = 1.0f;

    protected float _attackDelayCeof = 1.0f; //공격딜레이 계수
    /// <summary>몬스터 공격딜레이, 값 대입시 계수로 적용</summary>
    public float AttackDelay { get => Data.Sp * _attackDelayCeof; set => _attackDelayCeof = value; }
    public void ResetAttackDelay() => _attackDelayCeof = 1.0f;

    /// <summary>모든 상태변화 초기화</summary>
    public void ResetAllState()
    {
        ResetSpeed();
        ResetAttack();
        ResetAttackDelay();
    }


    public Monster(MonsterData data) => Data = data;


}
