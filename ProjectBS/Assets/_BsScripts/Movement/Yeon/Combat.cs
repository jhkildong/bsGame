using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    public class Combat : Movement, IDamage, IHealing, IBuffable
    {
        #region Public Field
        ////////////////////////////////Public Field////////////////////////////////
        public event UnityAction<float> ChangeHpAct = null;
        public UnityAction DeadAct;
        [SerializeField] protected Transform myAttackPoint;
        [SerializeField] protected LayerMask attackMask;
        #endregion

        #region Property
        ////////////////////////////////Property////////////////////////////////
        public float MaxHP => _maxHP;           //최대 체력
        protected float CurHp
        {
            get => _curHp;
            set
            {
                _curHp = value;
                ChangeHpAct?.Invoke((float)_curHp / (float)MaxHP);
            }
        }
        public float SpeedCeof => _speedCeof;   //이동속도 계수
        protected float Attack { get => _attack * getBuff.atkBuff; }
        public bool IsDead => CurHp <= 0;
        #endregion

        #region Private Field
        ////////////////////////////////Private Field////////////////////////////////
        [SerializeField] protected float _maxHP;                  //최대 체력
        [SerializeField] protected float _curHp;                  //현재 체력
        [SerializeField] protected float _speedCeof = 1.0f;       //이동속도 계수
        [SerializeField] protected float _attackCeof = 1.0f;      //공격력 계수
        [SerializeField] protected float _attack;                 //공격력


        #endregion

        #region DamageEffect
        ////////////////////////////////DamageEffect////////////////////////////////
        [System.Serializable]
        public class HitEffectData
        {
            public int effectCount = 3;
            public float effectTime = 0.1f;
            public Color effectColor = Color.red;
            public Renderer[] renderers;
            public Texture mainTexture;
            public Color mainColor;

            public void SetRenderer(Combat combat)
            {
                renderers = combat.gameObject.GetComponentsInChildren<Renderer>();
                mainTexture = renderers.Length > 0 ? renderers[0].material.mainTexture : null;
                mainColor = renderers.Length > 0 ? renderers[0].material.color : Color.white;
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
        public HitEffectData effectData => _effectData;
        [SerializeField]private HitEffectData _effectData = new();
        private Coroutine _onDamageEffect;
        #endregion

        #region Unity Event
        ////////////////////////////////UnityEvent////////////////////////////////
        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            _onDamageEffect = null;
            effectData.ChangeTexture(effectData.mainTexture);
            effectData.ChangeColor(effectData.mainColor);
        }
        #endregion

        #region Interface Method
        ////////////////////////////////InterfaceMethod////////////////////////////////
        public virtual void TakeDamage(float damage)
        {
            CurHp -= damage;
            if(_onDamageEffect == null && CurHp > 0.0f)
            {
                _onDamageEffect = StartCoroutine(OnDamageEffect());
            }
            //Debug.Log($"받은 데미지:{damage}, 현재 체력:{CurHp}");
            if (CurHp <= 0.0f)
            {
                DeadAct?.Invoke();
            }
        }
        public void ReceiveHeal(float heal)
        {
            Debug.Log(heal);
            if (IsDead)
                return;
            CurHp += heal;
            CurHp = Mathf.Clamp(CurHp, 0, MaxHP); 
        }

        public Buff getBuff { get => _buff; set => _buff = value; }
        private Buff _buff = new Buff();
        #endregion

        #region Private Method
        ////////////////////////////////PrivateMethod////////////////////////////////
        IEnumerator OnDamageEffect()
        {
            WaitForSeconds wait = new WaitForSeconds(effectData.effectTime);
            for(int i =0; i < effectData.effectCount; i++)
            {
                effectData.ChangeTexture(null);
                effectData.ChangeColor(effectData.effectColor);
                yield return wait;
                effectData.ChangeTexture(effectData.mainTexture);
                effectData.ChangeColor(effectData.mainColor);
                yield return wait;
            }
            _onDamageEffect = null;
        }
        #endregion
    }
}

