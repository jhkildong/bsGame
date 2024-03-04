using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yeon.ResetExtention
{
    public static class Reset
    {
        public static void ResetCeof(this ref float ceof) => ceof = 1.0f;
    }
}

public abstract class Monster
{
    public MonsterData Data { get; private set; }

    /// <summary> ���� ü�� </summary>
    public short HP { get; protected set; }
    /// <summary> �ִ�ü��</summary>
    public short MaxHP => Data.MaxHP;
    /// <summary> ���� ü���� 0���ϰ� �Ǹ� true  </summary>
    public bool IsDead => HP <= 0;
    public void TakeDamage(short damage) => HP -= damage;
    public void ReceiveHeal(short heal)
    {
        if (IsDead)
            return;
        HP += heal;
        Mathf.Clamp(HP, 0, MaxHP);
    }

    [SerializeField]
    protected float _speedCeof = 1.0f;
    /// <summary>���� �̵��ӵ�, �� ���Խ� ����� ����</summary>
    public float Speed { get => Data.Sp * _speedCeof;  set => _speedCeof = value; }

    protected float _attackCeof = 1.0f;
    /// <summary>���� ���ݷ�, �� ���Խ� ����� ����</summary>
    public float Attack { get => Data.Ak * _attackCeof; set => _attackCeof = value; }

    protected float _attackDelayCeof = 1.0f;
    /// <summary>���� ���ݵ�����, �� ���Խ� ����� ����</summary>
    public float AttackDelay { get => Data.Sp * _attackDelayCeof; set => _attackDelayCeof = value; }


    public Monster(MonsterData data) => Data = data;
}
