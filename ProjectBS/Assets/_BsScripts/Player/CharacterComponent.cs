using UnityEngine;
using UnityEngine.Animations.Rigging;

//플레이어블 캐릭터 컴포넌트 관리해주는 클래스
public abstract class CharacterComponent : MonoBehaviour
{
    #region Property
    public Transform MyTransform => this.transform;
    public Transform MyEffectSpawn 
    {
        get
        {
            if (_effectSpawn == null)
            {
                if(_effectSpawn = transform.Find("EffectSpawn"))
                {
                    GameObject go = new GameObject("EffectSpawn");
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(0, 0.7f, 1.5f);
                }
            }
            return _effectSpawn;
        }
    }
    public Transform MyAttackPoint
    {
        get
        {
            if (_attackPoint == null)
            {
                if (_attackPoint = transform.Find("AttackPoint"))
                {
                    GameObject go = new GameObject("AttackPoint");
                    go.transform.SetParent(transform);
                    go.transform.localPosition = Vector3.zero;
                }
            }
            return _attackPoint;
        }
    }
    public AnimEvent MyAnimEvent
    {
        get
        {
            if (_animEvent == null)
            {
                if (TryGetComponent(out _animEvent))
                {
                    _animEvent = gameObject.AddComponent<AnimEvent>();
                }
            }
            return _animEvent;
        }
    }
    public Animator MyAnim
    {
        get 
        {             
            if (_anim == null)
            {
                if (_anim = GetComponent<Animator>())
                    return null;
            }
            return _anim;
        }
    }
    public Rig[] MyRigs
    {
        get
        {
            if (_rigs == null)
            {
                if ((_rigs = GetComponentsInChildren<Rig>()) == null)
                    return null;
            }
            return _rigs;
        }
    }
    public SkinnedMeshRenderer Myrenderer
    {
        get
        {
            if (_renderer == null)
            {
                if ((_renderer = GetComponentInChildren<SkinnedMeshRenderer>()) == null)
                    return null;
            }
            return _renderer;
        }
    }
    public abstract Effect[] MyEffects{ get; }
    #endregion

    #region Private Field
    [SerializeField] protected Transform _effectSpawn;
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] protected AnimEvent _animEvent;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected Rig[] _rigs;
    [SerializeField] protected SkinnedMeshRenderer _renderer;
    [SerializeField] protected Effect[] _effects = new Effect[0];
    #endregion

    public void SetRigWeight(float weight)
    {
        foreach (Rig rig in MyRigs)
        {
            rig.weight = weight;
        }
    }
    public abstract Effect GetMyEffect();
}