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
        public short MaxHP => _maxHP;           //최대 체력
        protected short CurHp
        {
            get => _curHp;
            set
            {
                _curHp = value;
                ChangeHpAct?.Invoke((float)_curHp / (float)MaxHP);
            }
        }
        public float SpeedCeof => _speedCeof;   //이동속도 계수
        public float AttackCeof => _attackCeof; //공격력 계수
        protected short Attack { get => (short)(_attack * _attackCeof); }
        public bool IsDead => CurHp <= 0;
        #endregion

        #region Private Field
        [SerializeField] protected short _maxHP;                  //최대 체력
        [SerializeField] protected short _curHp;                  //현재 체력
        [SerializeField] protected float _speedCeof = 1.0f;       //이동속도 계수
        [SerializeField] protected float _attackCeof = 1.0f;      //공격력 계수
        [SerializeField] protected short _attack;                 //공격력
        #endregion

        #region Interface Method
        public virtual void TakeDamage(short damage)
        {
            CurHp -= damage;
            Debug.Log($"받은 데미지:{damage}, 현재 체력:{CurHp}");
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

