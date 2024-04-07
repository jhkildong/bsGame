using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    public class Combat : Movement, IDamage, IHealing
    {
        #region Public Field
        public event UnityAction<float> ChangeHpAct = null;
        public UnityAction DeadAct;
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

        protected int effectCount = 3;
        protected float effectTime = 0.1f;
        protected Color effctColor = Color.red;
        protected SkinnedMeshRenderer _myRenderer;
        protected Texture _myTexture;
        private Coroutine _onDamageEffect;
        #endregion

        #region Unity Event

        protected virtual void OnEnable()
        {
            if(_myRenderer == null)
            {
                _myRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
                _myTexture = _myRenderer.material.mainTexture;
            }

        }

        protected virtual void OnDisable()
        {
            if (_onDamageEffect != null)
            {
                _myRenderer.material.mainTexture = _myTexture;
                _myRenderer.material.color = Color.white;
                StopAllCoroutines();
            }
        }
        #endregion

        #region Interface Method
        public virtual void TakeDamage(short damage)
        {
            CurHp -= damage;
            if(_onDamageEffect == null)
            {
                _onDamageEffect = StartCoroutine(OnDamageEffect());
            }
            //Debug.Log($"���� ������:{damage}, ���� ü��:{CurHp}");
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

        #region Private Method
        IEnumerator OnDamageEffect()
        {
            WaitForSeconds wait = new WaitForSeconds(effectTime);
            for(int i =0; i < effectCount; i++)
            {

                _myRenderer.material.mainTexture = null;
                _myRenderer.material.color = effctColor;
                yield return wait;
                _myRenderer.material.mainTexture = _myTexture;
                _myRenderer.material.color = Color.white;
                yield return wait;
            }
            _onDamageEffect = null;
        }
        #endregion
    }
}

