using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Yeon;
using InfinityPBR;

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
    [SerializeField] private float _curAttackDelay;
    [SerializeField] private Animator myAnim;
    #endregion

    #region Interface Method
    public List<dropItem> dropItems() => OriginData.DropItemList;
    public void WillDrop()
    {
        //GameObject go = ItemManager.Instance.A(dropItems());
        //go.transform.position = this.transform.position;
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
        gameObject.layer = 15;

        _curAttackDelay = data.AkDelay;
        _curHp = data.MaxHP;
        moveSpeed = data.Sp;

        effectCount = 1;
        effctColor = Color.white;
        effectTime = 0.1f;

        //DeadAct.AddListener(WillDrop);
        Instantiate(data.Prefab, this.transform); //자식으로 몬스터의 프리팹 생성
        myAnim = GetComponentInChildren<Animator>();
        //임시
        PlayerTransform = GameObject.Find("Player").transform;
        myTarget = PlayerTransform;
        DeadAct += Die;
    }
    #endregion

    #region Unity Event
    protected override void Awake()
    {
        base.Awake();
        //dropTable = GetComponent<DropTable>();
    }

    //활성화 될 때 상태 초기화
    protected override void OnEnable()
    {
        base.OnEnable();
        if (OriginData == null) return;
        _curAttackDelay = OriginData.AkDelay;
        _curHp = OriginData.MaxHP;
        moveSpeed = OriginData.Sp;
        myTarget = PlayerTransform;
        ChangeState(State.Chase);
    }
    #endregion

    #region Private Method
    private void Die()
    {
        myAnim.SetBool(AnimParam.isMoving, false);
        myAnim.SetTrigger(AnimParam.Death);
        ChangeState(State.Death);
        //임시
        //DropTable dropTable = new DropTable();
        //dropTable.WillDrop(dropItems()).transform.position = this.transform.position + new Vector3(0f, 0.3f, 0f);
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
        Create, Chase, Attack, Death
    }
    [SerializeField]protected State myState = State.Create;
    [SerializeField]protected Transform myTarget;

    protected virtual void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                myAnim.SetBool(AnimParam.isMoving, true);
                break;
            case State.Death:
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
        }
    }

    private void Update()
    {
        //타겟을 향해 부드럽게 방향전환
        Vector3 targetDirection = myTarget.position - transform.position;
        targetDirection.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        Physics.Raycast(transform.position, targetDirection, out RaycastHit hit, 0.5f,
                        (int)BSLayerMasks.Player | (int)BSLayerMasks.Building);
        if(hit.collider != null)
        {
            
        }
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
            AttackTarget.TakeDamage(Attack);
            //ChangeState(State.Attack);
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
