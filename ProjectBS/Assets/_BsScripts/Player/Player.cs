using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamage
{
    public UnityEvent<float> ChangeHpAct;
    public UnityEvent DeadAct;
    public Transform myAttackPoint;
    public LayerMask monsterMask;

    public short CurHp
    {
        get => _curHp;
        set
        {
            if (value <= 0)
            {
                //죽은상태
                ChangeHpAct?.Invoke(0.0f);
                DeadAct?.Invoke();
                return;
            }
            _curHp = value;
            ChangeHpAct?.Invoke((float)_curHp / (float)MaxHP);
        }
    }           //현재 체력
    public short MaxHP => _maxHP;   //최대 체력
    public float SpeedCeof => _speedCeof;
    public float AttackCeof => _attackCeof;
    protected short Attack { get => (short)(_attack * _attackCeof); set => _attack = value; }

    [SerializeField] private short _curHp;
    [SerializeField] private short _maxHP;
    [SerializeField] private float _speedCeof = 1.0f; //이동속도 계수
    [SerializeField] private float _attackCeof = 1.0f; //공격력 계수
    [SerializeField] private short _attack;

    protected Animator myAnim;

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

    public void OnAttack()
    {
        Collider[] list = Physics.OverlapSphere(myAttackPoint.position, 1.0f, monsterMask);

        foreach (Collider col in list)
        {
            IDamage act = col.GetComponent<IDamage>();
            if (act != null)
            {
                act.TakeDamage(Attack);
            }
        }
    }

    protected void Init()
    {
        myAnim = GetComponentInChildren<Animator>();
        ChangeHpAct.AddListener(PlayerUI.Instance.ChangeHP);
        CurHp = MaxHP;
    }
}
