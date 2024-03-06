using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class Monster : MonoBehaviour
{
    #region ������ƮǮ
    public IObjectPool<Monster> Pool;

    public void ReleaseMonster()
    {
        Pool.Release(this);
    }
    #endregion

    public MonsterData Data { get; protected set; }
    public UnityEvent<float> ChangeHpAct;
    public UnityEvent DeadAct;

    /// <summary> ���� ü�� </summary>
    protected short CurHp
    {   
        get => _curHp;
        set
        {
            if (value <= 0 )
            {
                ReleaseMonster();
                DeadAct?.Invoke();
                value = 0;
            }    
            _curHp = value;
            ChangeHpAct?.Invoke(_curHp/MaxHP);
        }
    }
    [SerializeField] protected short _curHp;
    /// <summary> �ִ�ü��</summary>
    public short MaxHP => Data.MaxHP;
    /// <summary> ���� ü���� 0���ϰ� �Ǹ� true  </summary>
    public bool IsDead => CurHp <= 0;
    
    public void TakeDamage(short damage) => CurHp -= damage;
    public void ReceiveHeal(short heal)
    {
        if (IsDead)
            return;
        CurHp += heal;
        Mathf.Clamp(CurHp, 0, MaxHP);
    }

    protected float _speedCeof = 1.0f; //�̵��ӵ� ���
    /// <summary>���� �̵��ӵ�, �� ���Խ� ����� ����</summary>
    public float Speed { get => Data.Sp * _speedCeof;  set => _speedCeof = value; }
    public void ResetSpeed() => _speedCeof = 1.0f;

    protected float _attackCeof = 1.0f; //���ݷ� ���
    /// <summary>���� ���ݷ�, �� ���Խ� ����� ����</summary>
    public float Attack { get => Data.Ak * _attackCeof; set => _attackCeof = value; }
    public void ResetAttack() => _attackCeof = 1.0f;

    protected float _attackDelayCeof = 1.0f; //���ݵ����� ���
    /// <summary>���� ���ݵ�����, �� ���Խ� ����� ����</summary>
    public float AttackDelay { get => Data.Sp * _attackDelayCeof; set => _attackDelayCeof = value; }
    public void ResetAttackDelay() => _attackDelayCeof = 1.0f;

    /// <summary>��� ���º�ȭ �ʱ�ȭ</summary>
    public void ResetAllState()
    {
        ResetSpeed();
        ResetAttack();
        ResetAttackDelay();
    }

    public abstract void Init(MonsterData data);
}
