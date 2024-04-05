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
        private Renderer[] _myRenderers;
        private Texture _myTexture;
        private Coroutine _onDamageEffect;
        #endregion

        #region Unity Event
        protected virtual void OnEnable()
        {
            _myRenderers = GetComponentsInChildren<Renderer>();
            if(_myRenderers.Length != 0)
                _myTexture = _myRenderers[0].material.mainTexture;
        }        
        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            foreach (Renderer renderer in _myRenderers)
            {
                renderer.material.mainTexture = _myTexture;
                renderer.material.color = Color.white;
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
                foreach(Renderer renderer in _myRenderers)
                {
                    renderer.material.mainTexture = null;
                    renderer.material.color = effctColor;
                }
                yield return wait;
                foreach (Renderer renderer in _myRenderers)
                {
                    renderer.material.mainTexture = _myTexture;
                    renderer.material.color = Color.white;
                }
                yield return wait;
            }
            _onDamageEffect = null;
        }
        #endregion
    }
}

