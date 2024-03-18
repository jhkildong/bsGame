using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    public class Combat : Movement, IDamage, IHealing
    {
        #region SerializeField
        [SerializeField] protected UnityEvent<float> ChangeHpAct;
        [SerializeField] protected UnityEvent DeadAct;
        [SerializeField] protected Transform myAttackPoint;
        [SerializeField] protected LayerMask attackMask;
        #endregion

        public short MaxHP => _maxHP;           //최대 체력
        public float SpeedCeof => _speedCeof;   //이동속도 계수
        public float AttackCeof => _attackCeof; //공격력 계수
        protected short Attack { get => (short)(_attack * _attackCeof); set => _attack = value; }

        [SerializeField] private short _maxHP;
        [SerializeField] private float _speedCeof = 1.0f; 
        [SerializeField] private float _attackCeof = 1.0f;
        [SerializeField] private short _attack;


        protected short CurHp
        {
            get => _curHp;
            set
            {
                _curHp = value;
                ChangeHpAct?.Invoke((float)_curHp / (float)MaxHP);
            }
        }
        [SerializeField] private short _curHp; //현재 체력

        #region Public Method
        /// <summary> 현재 체력이 0이하가 되면 true  </summary>
        public bool IsDead() => CurHp <= 0;
        
        public void TakeDamage(short damage)
        {
            CurHp -= damage;
            if (CurHp <= 0.0f)
            {
                DeadAct?.Invoke();
            }
        }
        
        public void ReceiveHeal(short heal)
        {
            if (IsDead())
                return;
            CurHp += heal;
            CurHp = (short)Mathf.Clamp(CurHp, 0, MaxHP);
        }
        
        public void OnAttackPoint()
        {
            Collider[] list = Physics.OverlapSphere(myAttackPoint.position, 1.0f, attackMask);

            foreach (Collider col in list)
            {
                IDamage act = col.GetComponent<IDamage>();
                if (act != null)
                {
                    act.TakeDamage(Attack);
                }
            }
        }
        #endregion
    }
}

