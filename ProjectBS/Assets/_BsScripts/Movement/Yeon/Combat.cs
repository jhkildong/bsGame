using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    public class Combat : Movement, IDamage<Combat>, IHealing
    {
        #region Public Field
        public event UnityAction<float> ChangeHpAct = null;
        public event UnityAction DeadAct;
        [SerializeField] protected Transform myAttackPoint;
        [SerializeField] protected LayerMask attackMask;
        #endregion

        #region Property
        public short MaxHP => _maxHP;           //�ִ� ü��
        protected short CurHp
        {
            get => _curHp;
            set
            {
                _curHp = value;
                ChangeHpAct?.Invoke((float)_curHp / (float)MaxHP);
            }
        }
        public float SpeedCeof => _speedCeof;   //�̵��ӵ� ���
        public float AttackCeof => _attackCeof; //���ݷ� ���
        protected short Attack { get => (short)(_attack * _attackCeof); }
        public bool IsDead => CurHp <= 0;
        #endregion

        #region Private Field
        [SerializeField] protected short _maxHP;                  //�ִ� ü��
        [SerializeField] protected short _curHp;                  //���� ü��
        [SerializeField] protected float _speedCeof = 1.0f;       //�̵��ӵ� ���
        [SerializeField] protected float _attackCeof = 1.0f;      //���ݷ� ���
        [SerializeField] protected short _attack;                 //���ݷ�
        #endregion

        #region Interface Method
        public virtual void TakeDamage(short damage)
        {
            CurHp -= damage;
            Debug.Log($"���� ������:{damage}, ���� ü��:{CurHp}");
            if (CurHp <= 0.0f)
            {
                DeadAct?.Invoke();
            }
        }
        public void ReceiveHeal(short heal)
        {
            Debug.Log(heal);
            if (IsDead)
                return;
            CurHp += heal;
            CurHp = (short)Mathf.Clamp(CurHp, 0, MaxHP); 
        }
        #endregion

        #region Public Method


        #endregion
    }
}

