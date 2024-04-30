using UnityEngine;
using UnityEngine.Animations.Rigging;


//캐릭터 컴포넌트 관리해주는 클래스
public abstract class PlayerComponent : CharacterComponent
{
    protected static class EffectID
    {
        public const int Warrior = 3500;
        public const int Archer = 3510;
        public const int Mage = 3520;
    }

    #region Property
    ////////////////////////////////Property////////////////////////////////
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
    public float Attack
    {
        get => _attack;
        set => _attack = value;
    }
    public virtual Effect MyEffect
    { 
        get => _effect;
        set => _effect = value;
    }
    #endregion

    #region Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] protected Transform _effectSpawn;
    [SerializeField] protected Rig[] _rigs;
    [SerializeField] protected float _attack;
    [SerializeField] protected Effect[] _effects;
    [SerializeField] protected int _effectIdx;
    [SerializeField] protected Effect _effect;

    
    #endregion

    #region Abstract Method
    ////////////////////////////////AbstractMethod////////////////////////////////
    public abstract void OnAttackPoint();           //_effectSpawn에 공격 생성(애니메이션에서 호출)
    #endregion

    #region Public Method
    ////////////////////////////////PublicMethod////////////////////////////////
    public void SetRigWeight(float weight)
    {
        foreach (Rig rig in MyRigs)
        {
            rig.weight = weight;
        }
    }
    #endregion
}