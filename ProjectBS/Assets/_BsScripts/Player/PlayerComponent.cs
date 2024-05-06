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
                if (_effectSpawn = transform.Find("EffectSpawn"))
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
    public virtual PlayerAttackType MyEffect
    {
        get => _effect;
        set => _effect = value;
    }
    public PlayerSkill MySkillEffect { get => _skillEffect; set => _skillEffect = value; }
    public PlayerStat MyStat => _playerStat;
    public JobBless MyJobBless { get => _jobBless; set => _jobBless = value;}
    #endregion

    #region Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] protected Transform _effectSpawn;
    [SerializeField] protected Rig[] _rigs;
    [SerializeField] protected float _attack;
    [SerializeField] protected int _effectIdx;
    [SerializeField] protected PlayerAttackType _effect;
    [SerializeField] protected PlayerSkill _skillEffect;
    [SerializeField] protected PlayerStat _playerStat = new PlayerStat();
    [SerializeField] protected JobBless _jobBless;


    [SerializeField] protected Transform weaponPoint;
    [SerializeField] protected Transform hammerPoint;

    private void ChangeWeapon(int idx)
    {
        switch (idx)
        {
            case 0:
                weaponPoint.gameObject.SetActive(true);
                hammerPoint.gameObject.SetActive(false);
                break;
            case 1:
                weaponPoint.gameObject.SetActive(false);
                hammerPoint.gameObject.SetActive(true);
                break;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        MyAnimEvent.ChangeWeaponAct += ChangeWeapon;
    }
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

    //애니메이션 이벤트는 bool값을 지원안함
    public virtual void OnSkillEffect(int onSkill)
    {
        if (onSkill == 0)
            MySkillEffect.gameObject.SetActive(false);
        else if (onSkill == 1)
            MySkillEffect.gameObject.SetActive(true);
    }
    #endregion
}

public enum Job
{
    Warrior,
    Archer,
    Mage
}