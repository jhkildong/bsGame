using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeon;

[RequireComponent(typeof(DropTable))]
public abstract class Monster : Combat, IDropable, IDamage<Monster>, IPoolable
{
    #region Public Field
    //임시
    public event UnityAction<Transform> DeadTransformAct;
    #endregion

    #region Property
    ////////////////////////////////Property////////////////////////////////
    public MonsterComponent Com => _monsterComponent;
    public MonsterData Data => _data;
    public float AttackDelay => Data.AkDelay;
    #endregion

    #region Private Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] protected MonsterComponent _monsterComponent;
    [SerializeField] protected MonsterData _data;
    [SerializeField] protected DropTable dropTable;
    #endregion

    #region Interface Method
    ////////////////////////////////InterfaceMethod////////////////////////////////
    public List<dropItem> dropItems() => Data.DropItemList;
    public void WillDrop()
    {
        GameObject go = ItemManager.Instance.A(dropItems());
        go.transform.position = this.transform.position + new Vector3(0f, 0.3f, 0f);
    }

    public override void TakeDamage(float damage)
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
        return Data.CreateClone();
    }
    public int ID => Data.ID;
    public MonoBehaviour This => this;
    #endregion

    #region Init Method
    ////////////////////////////////InitMethod////////////////////////////////
    public virtual void Init(MonsterData data)
    {
        _data = data;
        
        Instantiate(data.Prefab, this.transform); //자식으로 몬스터의 프리팹 생성

        _maxHP = data.MaxHP;
        _attack = data.Ak;

        _curHp = data.MaxHP;
        moveSpeed = data.Sp;

        attackMask = (int)(BSLayerMasks.Player | BSLayerMasks.Building);
        gameObject.layer = (int)BSLayers.Monster;

        rBody.mass = data.Mass;
        rBody.angularDrag = 2f;
        SetCollider(data.Radius);

        _monsterComponent = GetComponentInChildren<MonsterComponent>();

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

    private void SetCollider(float radius)
    {
        if (radius < 1.0f)
        {
            col = gameObject.AddComponent<CapsuleCollider>();
            CapsuleCollider cc = col as CapsuleCollider;
            cc.radius = radius;
            cc.height = 2.0f;
            cc.center = new Vector3(0, 1.0f, 0);
        }
        else
        {
            col = gameObject.AddComponent<SphereCollider>();
            SphereCollider sc = col as SphereCollider;
            sc.radius = radius;
            sc.center = new Vector3(0, radius, 0);
        }
    }

    #endregion

    #region Unity Event
    ////////////////////////////////UnityEvent////////////////////////////////
    protected override void Awake()
    {
        InitRigidbody();
        dropTable = GetComponent<DropTable>();
    }

    //활성화 될 때 상태 초기화
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Data == null) return;

        _curHp = Data.MaxHP;
        moveSpeed = Data.Sp;
        ResetTarget();
        ChangeState(State.Chase);
    }

    #endregion

    #region Private Method
    ////////////////////////////////PrivateMethod////////////////////////////////
    private void Die()
    {
        Com.MyAnim.SetTrigger(AnimParam.Death);
        ChangeState(State.Death);
        dropTable.WillDrop(Data.DropItemList).transform.position = this.transform.position + Vector3.up;   //아이템생성
    }

    protected virtual void ChangeTarget(Transform target)
    {
        if (myState == State.Death) return;
        myTarget = target;
    }

    protected virtual void ResetTarget()
    {
        if (myState == State.Death) return;
        myTarget = PlayerTransform;
    }
    #endregion

    #region StateMachine
    ////////////////////////////////StateMachine////////////////////////////////
    public enum State
    {
        Chase, Attack, Death
    }
    [SerializeField]protected State myState = State.Chase;
    [SerializeField]protected Transform myTarget;
    [SerializeField]protected IDamage attackTarget;
    [SerializeField]protected float playTime;


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
                playTime = Data.AkDelay;
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
                SetDirection(transform.forward);
                break;
            case State.Attack:
                playTime -= Time.deltaTime;
                if(playTime <= 0)
                {
                    attackTarget.TakeDamage(Attack);
                    playTime = Data.AkDelay;
                }
                break;
            case State.Death:
                break;
        }
    }

    protected virtual void Update()
    {
        StateProcess();

        if (myState == State.Death) return;
        //타겟을 향해 부드럽게 방향전환
        Vector3 targetDirection = myTarget.position - transform.position;
        targetDirection.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion

    #region Collision Event
    ////////////////////////////////CollisionEvent////////////////////////////////
    protected virtual void OnCollisionEnter(Collision collision)
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

    protected virtual void OnCollisionExit(Collision collision)
    {
        if ((attackMask & (1 << collision.gameObject.layer)) != 0)
        {
            ChangeState(State.Chase);
        }
    }
    #endregion
}
