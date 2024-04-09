using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeon;

[RequireComponent(typeof(DropTable))]
public abstract class Monster : Combat, IDropable, IDamage<Monster>, IPoolable
{
    #region Public Field
    public event UnityAction<Transform> DeadTransformAct;
    #endregion

    #region Property
    public MonsterData OriginData => _data;
    public float AttackDelay => OriginData.AkDelay;
    public float CurAttackDelay { get => _curAttackDelay; set => _curAttackDelay = value; }
    public bool isAttack => CurAttackDelay < AttackDelay;
    #endregion

    #region Private Field
    [SerializeField] protected MonsterData _data;
    [SerializeField] protected float _curAttackDelay;
    [SerializeField] protected Animator myAnim;
    [SerializeField] protected DropTable dropTable;
    #endregion

    #region Interface Method
    public List<dropItem> dropItems() => OriginData.DropItemList;
    public void WillDrop()
    {
        GameObject go = ItemManager.Instance.A(dropItems());
        go.transform.position = this.transform.position + new Vector3(0f, 0.3f, 0f);
    }

    public override void TakeDamage(short damage)
    {
        base.TakeDamage(damage);
        if (CurHp <= 0)
        {
            DeadTransformAct?.Invoke(this.transform);
        }
    }
    //임시
    protected Transform PlayerTransform;

    public IPoolable CreateClone()
    {
        return OriginData.CreateClone();
    }
    public int ID => OriginData.ID;
    public MonoBehaviour Data => this;
    #endregion

    #region Init Method
    public virtual void Init(MonsterData data)
    {
        _data = data;
        _maxHP = data.MaxHP;
        attackMask = (int)(BSLayerMasks.Player | BSLayerMasks.Building);
        _attack = (short)data.Ak;
        gameObject.layer = (int)BSLayers.Monster;

        _curAttackDelay = data.AkDelay;
        _curHp = data.MaxHP;
        moveSpeed = data.Sp;

        rBody.mass = data.Mass;
        rBody.angularDrag = 2f;
        SetCollider(data.Radius);

        Instantiate(data.Prefab, this.transform); //자식으로 몬스터의 프리팹 생성
        myAnim = GetComponentInChildren<Animator>();

        //타격 이펙트 설정
        effectData.effectCount = 1;
        effectData.effectColor = Color.white;
        effectData.effectTime = 0.1f;
        effectData.SetRenderer(this);

        //임시
        PlayerTransform = GameObject.Find("Player").transform;
        ResetTarget();
        DeadAct += Die;
    }
    #endregion

    #region Unity Event
    protected override void Awake()
    {
        base.Awake();
        
        dropTable = GetComponent<DropTable>();
    }

    //활성화 될 때 상태 초기화
    protected override void OnEnable()
    {
        base.OnEnable();
        if (OriginData == null) return;

        _curAttackDelay = OriginData.AkDelay;
        _curHp = OriginData.MaxHP;
        moveSpeed = OriginData.Sp;
        ResetTarget();
        ChangeState(State.Chase);
    }

    #endregion

    #region Private Method
    private void Die()
    {
        myAnim.SetTrigger(AnimParam.Death);
        ChangeState(State.Death);
        dropTable.WillDrop(OriginData.DropItemList).transform.position = this.transform.position + Vector3.up;   //아이템생성
    }

    protected void ChangeTarget(Transform target)
    {
        if (myState == State.Death) return;
        myTarget = target;
        ChangeState(State.Chase);

    }

    protected void ResetTarget()
    {
        if (myState == State.Death) return;
        myTarget = PlayerTransform;
    }
    #endregion

    #region StateMachine
    public enum State
    {
        Chase, Attack, Death
    }
    [SerializeField]protected State myState = State.Chase;
    [SerializeField]protected Transform myTarget;
    [SerializeField] protected IDamage attackTarget;
    [SerializeField] protected float playTime;


    protected virtual void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                break;
            case State.Attack:
                SetDirection(Vector3.zero);
                attackTarget.TakeDamage(Attack);
                playTime = CurAttackDelay;
                break;
            case State.Death:
                SetDirection(Vector3.zero);
                ObjectPoolManager.Instance.ReleaseObj(this);
                break;
        }
    }

    protected virtual void StateProcess()
    {
        switch (myState)
        {
            case State.Chase:
                Vector3 dir = transform.forward;
                dir.y = 0;
                SetDirection(dir);
                break;
            case State.Attack:
                playTime -= Time.deltaTime;
                if(playTime <= 0)
                {
                    attackTarget.TakeDamage(Attack);
                    playTime = CurAttackDelay;
                }
                break;
            case State.Death:
                break;
        }
    }

    protected virtual void Update()
    {
        if (myState == State.Death) return;
        //타겟을 향해 부드럽게 방향전환
        Vector3 targetDirection = myTarget.position - transform.position;
        targetDirection.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        StateProcess();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion

    #region Collision Event
    private void OnCollisionEnter(Collision collision)
    {
        if ((attackMask & (1 << collision.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = collision.gameObject.GetComponent<IDamage>();
            if(AttackTarget != null)
            {
                attackTarget = AttackTarget;
                ChangeState(State.Attack);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((attackMask & (1 << collision.gameObject.layer)) != 0)
        {
            ChangeState(State.Chase);
        }
    }
    #endregion
}
