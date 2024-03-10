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
                //��������
                ChangeHpAct?.Invoke(0.0f);
                DeadAct?.Invoke();
                return;
            }
            _curHp = value;
            ChangeHpAct?.Invoke((float)_curHp / (float)MaxHP);
        }
    }           //���� ü��
    public short MaxHP => _maxHP;   //�ִ� ü��
    public float SpeedCeof => _speedCeof;
    public float AttackCeof => _attackCeof;
    protected short Attack { get => (short)(_attack * _attackCeof); set => _attack = value; }

    [SerializeField] private short _curHp;
    [SerializeField] private short _maxHP;
    [SerializeField] private float _speedCeof = 1.0f; //�̵��ӵ� ���
    [SerializeField] private float _attackCeof = 1.0f; //���ݷ� ���
    [SerializeField] private short _attack;

    protected Animator myAnim;

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
