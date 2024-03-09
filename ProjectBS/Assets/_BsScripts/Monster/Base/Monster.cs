using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(MonsterFollowPlayer))]
[RequireComponent(typeof(MonsterAction))]
public abstract class Monster : MonoBehaviour, IDamage
{
    #region ������ƮǮ
    public IObjectPool<Monster> Pool;

    public void ReleaseMonster()
    {
        Pool.Release(this);
    }
    #endregion

    public MonsterData Data { get; private set; }
    public UnityEvent<float> ChangeHpAct;
    public UnityEvent DeadAct;
    MonsterFollowPlayer mfp;

    private bool _isBlocked;
    public bool isBlocked 
    {
        get => _isBlocked;
        set
        {
            if(mfp == null)
            {
                TryGetComponent(out mfp);
                Debug.Log("Block");
            }
            mfp.block(value);
            _isBlocked = value;
        }
    }

    /// <summary> ���� ü�� </summary>
    private short CurHp
    {   
        get => _curHp;
        set
        {
            if (value <= 0 )
            {
                //��������
                ReleaseMonster();
                DeadAct?.Invoke();
                value = 0;
            }    
            _curHp = value;
            ChangeHpAct?.Invoke(_curHp/MaxHP);
        }
    }
    [SerializeField] private short _curHp;
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

    private float _speedCeof = 1.0f; //�̵��ӵ� ���
    public float SpeedCeof { get =>_speedCeof ; set => _speedCeof = value; }
    /// <summary>���� �̵��ӵ�</summary>
    public float Speed => Data.Sp * _speedCeof;
    public void ResetSpeed() => _speedCeof = 1.0f;

    private float _attackCeof = 1.0f; //���ݷ� ���
    public float AttackCeof { get => _attackCeof; set => _attackCeof = value; }
    /// <summary>���� ���ݷ�</summary>
    public float Attack => Data.Ak * _attackCeof;
    public void ResetAttack() => _attackCeof = 1.0f;

    /// <summary>���� ���ݵ�����</summary>
    public float AttackDelay => Data.AkDelay;
    public void ResetAttackDelay() => _curAttackDelay = Data.AkDelay;

    private float _curAttackDelay;
    public float CurAttackDelay{ get => _curAttackDelay; set => _curAttackDelay = value; }
    public bool isAttack => CurAttackDelay < AttackDelay;

    /// <summary>��� ���º�ȭ �ʱ�ȭ</summary>
    public void ResetAllState()
    {
        ResetSpeed();
        ResetAttack();
        ResetAttackDelay();
    }

    public virtual void Init(MonsterData data)
    {
        Data = data;
        CurHp = MaxHP;
        CurAttackDelay = AttackDelay;
        Instantiate(data.MonsterPrefab, this.transform); //�ڽ����� ������ ������ ����
    }
}
