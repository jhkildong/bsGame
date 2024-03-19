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

    /// <summary> 현재 체력 </summary>
    public short CurHp
    {   
        get => _curHp;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        set
        {
            if (value <= 0 )
            {
                //죽은상태
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
    /// <summary> 최대체력</summary>
    public short MaxHP => Data.MaxHP;
    /// <summary> 현재 체력이 0이하가 되면 true  </summary>
    public bool IsDead => CurHp <= 0;
    public void TakeDamage(short damage) => CurHp -= damage;
    public void ReceiveHeal(short heal)
    {
        if (IsDead)
            return;
        CurHp += heal;
        Mathf.Clamp(CurHp, 0, MaxHP);
    }

    [SerializeField]private float _speedCeof = 1.0f; //이동속도 계수
    public float SpeedCeof =>_speedCeof ;
    /// <summary>몬스터 이동속도</summary>
    public float Speed => Data.Sp * _speedCeof;
    public void ResetSpeed() => _speedCeof = 1.0f;

    [SerializeField]private float _attackCeof = 1.0f; //공격력 계수
    public float AttackCeof { get => _attackCeof; set => _attackCeof = value; }
    /// <summary>몬스터 공격력</summary>
    public float Attack => Data.Ak * _attackCeof;
    public void ResetAttack() => _attackCeof = 1.0f;

    /// <summary>몬스터 공격딜레이</summary>
    public float AttackDelay => Data.AkDelay;
    public void ResetAttackDelay() => _curAttackDelay = Data.AkDelay;

    [SerializeField]private float _curAttackDelay;
    public float CurAttackDelay{ get => _curAttackDelay; set => _curAttackDelay = value; }
    public bool isAttack => CurAttackDelay < AttackDelay;

    /// <summary>모든 상태변화 초기화</summary>
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
        Instantiate(data.Prefab, this.transform); //자식으로 몬스터의 프리팹 생성
    }
}
