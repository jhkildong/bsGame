using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations.Rigging;

//캐릭터 컴포넌트 관리해주는 클래스
public abstract class PlayerComponent : CharacterComponent
{
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
    public PlayerStat MyStat => _playerStat;
    #endregion

    #region Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] protected Transform _effectSpawn;
    [SerializeField] protected Rig[] _rigs;
    [SerializeField] protected float _attack;
    [SerializeField] protected Effect[] _effects;
    [SerializeField] protected int _effectIdx;
    [SerializeField] protected Effect _effect;
    [SerializeField] protected PlayerStat _playerStat = new PlayerStat();
    #endregion

    #region Abstract Method
    ////////////////////////////////AbstractMethod////////////////////////////////
    public abstract void OnAttackPoint();           //_effectSpawn에 공격 생성(애니메이션에서 호출)
    public abstract Job MyJob { get; }
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
    public virtual void SetSkillAct(Player player)
    {

    }
    #endregion
}

public enum Job
{
    [Description("전사")]
    Warrior,
    [Description("궁수")]
    Archer,
    [Description("마법사")]
    Mage
}

/// <summary>
/// Description을 가져오는 확장메서드 TODO?: 다른 Enum에도 사용할 수 있게 확장
/// </summary>
public static class JobExtension
{
    public static string GetDescription(this Job job)
    {
        FieldInfo fi = job.GetType().GetField(job.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : job.ToString();
    }
}