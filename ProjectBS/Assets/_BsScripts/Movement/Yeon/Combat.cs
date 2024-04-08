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


        #endregion

        #region DamageEffect
        [System.Serializable]
        public class EffectData
        {
            public int effectCount = 3;
            public float effectTime = 0.1f;
            public Color effectColor = Color.red;
            public Renderer[] renderers;
            public Texture mainTexture;

            public void SetRenderer(Combat combat)
            {
                renderers = combat.gameObject.GetComponentsInChildren<Renderer>();
                mainTexture = renderers.Length > 0 ? renderers[0].material.mainTexture : null;
            }

            public void ChangeTexture(Texture texture)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.material.mainTexture = texture;
                }
            }
            public void ChangeColor(Color color)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.material.color = color;
                }
            }
        }
        public EffectData effectData => _effectData;
        [SerializeField]private EffectData _effectData = new();
        private Coroutine _onDamageEffect;
        #endregion

        #region Unity Event

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            effectData.ChangeTexture(effectData.mainTexture);
            effectData.ChangeColor(Color.white);
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
            WaitForSeconds wait = new WaitForSeconds(effectData.effectTime);
            for(int i =0; i < effectData.effectCount; i++)
            {
                effectData.ChangeTexture(null);
                effectData.ChangeColor(effectData.effectColor);
                yield return wait;
                effectData.ChangeTexture(effectData.mainTexture);
                effectData.ChangeColor(Color.white);
                yield return wait;
            }
            _onDamageEffect = null;
        }
        #endregion
    }
}

