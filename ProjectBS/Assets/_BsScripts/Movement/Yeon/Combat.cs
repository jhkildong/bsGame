using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    public class Combat : Movement, IDamage, IHealing
    {
        #region SerializeField
        public event UnityAction<float> ChangeHpAct = null;
        public event UnityAction DeadAct;
        [SerializeField] protected Transform myAttackPoint;
        [SerializeField] protected LayerMask attackMask;
        #endregion

        public short MaxHP => _maxHP;           //�ִ� ü��
        public float SpeedCeof => _speedCeof;   //�̵��ӵ� ���
        public float AttackCeof => _attackCeof; //���ݷ� ���
        protected short Attack { get => (short)(_attack * _attackCeof); set => _attack = value; }

        [SerializeField] protected short _maxHP;
        [SerializeField] protected float _speedCeof = 1.0f; 
        [SerializeField] protected float _attackCeof = 1.0f;
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
        [SerializeField] private short _curHp; //���� ü��

        #region Public Method
        /// <summary> ���� ü���� 0���ϰ� �Ǹ� true  </summary>
        public bool IsDead() => CurHp <= 0;
        
        public void TakeDamage(short damage)
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

