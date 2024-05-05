using System.Buffers.Text;
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
        public float MaxHp
        {
            get => _maxHp * (1 + getBuff.hpBuff);
            set
            {
                _maxHp = value;
                tempMaxHp = _maxHp * (1 + getBuff.hpBuff);
            }
        }
        public float CurHp
        {
            get => _curHp;
            set
            {
                _curHp = value;
                ChangeHpAct?.Invoke(_curHp / MaxHp);
            }
        }
        protected float Aksp
        {
            get => _atkSpeed * (1 + getBuff.asBuff);
            set => _atkSpeed = value;
        }
        protected float Attack { get => Mathf.Round(_attack * (1 + getBuff.atkBuff) + additonalAtk); }
        
        public bool IsDead => CurHp <= 0;
        #endregion

        #region Private Field
        ////////////////////////////////Private Field////////////////////////////////
        [SerializeField] protected float _maxHp;                  //최대 체력
        [SerializeField] protected float tempMaxHp; 
        [SerializeField] protected float _curHp;                  //현재 체력
        [SerializeField] protected float _attack;                 //공격력
        [SerializeField] protected float _atkSpeed;               //공격속도


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
            public Texture[] mainTextures;
            public Color[] mainColor;

            public void SetRenderer(Renderer[] renderers)
            {
                if (renderers.Length == 0)  //렌더러가 없으면 리턴
                    return;
                this.renderers = renderers;
                mainTextures = new Texture[renderers.Length];
                mainColor = new Color[renderers.Length];
                for(int i = 0; i < renderers.Length; i++)
                {
                    mainTextures[i] = renderers[i].material.mainTexture;
                    mainColor[i] = renderers[i].material.color;
                }
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

            public void ResetTextureColor()
            {
                for(int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material.mainTexture = mainTextures[i];
                    renderers[i].material.color = mainColor[i];
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
            if(effectData.renderers != null)
                effectData.ResetTextureColor();
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
            Debug.Log($"받은 데미지:{damage}, 현재 체력:{CurHp}");
            if (CurHp <= 0.0f)
            {
                DeadAct?.Invoke();
            }
        }
        public virtual float Height
        {
            get
            {
                if (_height == 0.0f)
                {
                    if(col is CapsuleCollider cc)
                    {
                        _height = cc.height;
                    }
                    else if(col is BoxCollider bc)
                    {
                        _height = bc.size.y;
                    }
                    else if(col is SphereCollider sc)
                    {
                        _height = sc.radius * 2;
                    }
                }
                return _height;
            }
        }

        private float _height;
        public void ReceiveHeal(float heal)
        {
            Debug.Log(heal);
            if (IsDead)
                return;
            CurHp += heal;
            CurHp = Mathf.Clamp(CurHp, 0, MaxHp);
        }

        public Buff getBuff { get => _buff; set => _buff = value; }
        protected Buff _buff = new Buff();
        public float additonalAtk;
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
                effectData.ResetTextureColor();
                yield return wait;
            }
            _onDamageEffect = null;
        }
        #endregion
    }
}

