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
[RequireComponent(typeof(DropTable))]
public abstract class Monster : Yeon.Movement, IDamage, IDropable
{
    [SerializeField] private MonsterData _data;
    public MonsterData Data => _data;
    public UnityEvent<float> ChangeHpAct;
    public UnityEvent DeadAct;
    public List<dropItem> dropItems() => Data.DropItemList;
    public void WillDrop()
    {
        GameObject go = ItemManager.Instance.A(dropItems());
        go.transform.position = this.transform.position;
    }

    /// <summary> ���� ü�� </summary>
    public short CurHp
    {   
        get => _curHp;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        set
        {
            if (value <= 0 )
            {
                //��������
                //DropItem?.Invoke(this.transform.position);
                ObjectPoolManager.Instance.ReleaseObj(Data, gameObject);
                ChangeHpAct?.Invoke(0.0f);
                DeadAct?.Invoke();
                ResetAllState();
                //Data.dropTable.WillDrop();
                return;
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

    [SerializeField]private float _speedCeof = 1.0f; //�̵��ӵ� ���
    public float SpeedCeof =>_speedCeof ;
    /// <summary>���� �̵��ӵ�</summary>
    public float Speed => Data.Sp * _speedCeof;
    public void ResetSpeed() => _speedCeof = 1.0f;

    [SerializeField]private float _attackCeof = 1.0f; //���ݷ� ���
    public float AttackCeof { get => _attackCeof; set => _attackCeof = value; }
    /// <summary>���� ���ݷ�</summary>
    public float Attack => Data.Ak * _attackCeof;
    public void ResetAttack() => _attackCeof = 1.0f;

    /// <summary>���� ���ݵ�����</summary>
    public float AttackDelay => Data.AkDelay;
    public void ResetAttackDelay() => _curAttackDelay = Data.AkDelay;

    [SerializeField]private float _curAttackDelay;
    public float CurAttackDelay{ get => _curAttackDelay; set => _curAttackDelay = value; }
    public bool isAttack => CurAttackDelay < AttackDelay;

    /// <summary>��� ���º�ȭ �ʱ�ȭ</summary>
    public void ResetAllState()
    {
        _curAttackDelay = AttackDelay;
        ResetSpeed();
        ResetAttack();
        ResetAttackDelay();
    }
    public virtual void Init(MonsterData data)
    {
        _data = data;
        _curHp = MaxHP;
        _curAttackDelay = AttackDelay;
        gameObject.layer = 15;
        DeadAct.AddListener(WillDrop);
        Instantiate(data.Prefab, this.transform); //�ڽ����� ������ ������ ����
    }
}
