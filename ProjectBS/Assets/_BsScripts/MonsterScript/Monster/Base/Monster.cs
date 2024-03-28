using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Yeon;

[RequireComponent(typeof(DropTable))]
public abstract class Monster : Combat, IDropable, IDamage<Monster>
{
    #region Public Field
    public event UnityAction<Transform> DeadTransformAct;
    #endregion

    #region Property
    public MonsterData Data => _data;
    public float AttackDelay => Data.AkDelay;
    public float CurAttackDelay { get => _curAttackDelay; set => _curAttackDelay = value; }
    public bool isAttack => CurAttackDelay < AttackDelay;
    #endregion

    #region Private Field
    [SerializeField] protected MonsterData _data;
    [SerializeField] private float _curAttackDelay;
    #endregion

    #region Interface Method
    public List<dropItem> dropItems() => Data.DropItemList;
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
    Transform PlayerTransform;
    #endregion

    #region Init Method
    public virtual void Init(MonsterData data)
    {
        _data = data;
        _maxHP = data.MaxHP;
        _curAttackDelay = data.AkDelay;
        moveSpeed = data.Sp;
        CurHp = data.MaxHP;
        attackMask = (int)(BSLayerMasks.Player | BSLayerMasks.Building);
        gameObject.layer = (int)Mathf.Log((int)BSLayerMasks.Monster, 2);
        //DeadAct.AddListener(WillDrop);
        Instantiate(data.Prefab, this.transform); //자식으로 몬스터의 프리팹 생성
        //임시
        PlayerTransform = GameObject.Find("Player").transform;
        myTarget = PlayerTransform;
        DeadAct += Death;
    }
    #endregion

    #region Unity Start Event
    protected override void Awake()
    {
        base.Awake();
        //ChangeState(State.Chase);
        //dropTable = GetComponent<DropTable>();
    }

    protected virtual void OnEnable()
    {
        if (Data == null) return;
        moveSpeed = Data.Sp;
        CurHp = Data.MaxHP;
        playTime = 0.0f;
        ChangeState(State.Chase);
    }
    #endregion

    #region Private Method
    
    void Death()
    {
        myAnim.SetBool(AnimParam.isMoving, false);
        myAnim.SetTrigger(AnimParam.Death);
        ChangeState(State.Death);
        //임시
        DropTable dropTable = new DropTable();
        dropTable.WillDrop(dropItems()).transform.position = this.transform.position + new Vector3(0f, 0.3f, 0f);
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

    #region Monster StateMachine
    public enum State
    {
        Create, Chase, Attack, Death
    }
    public State myState = State.Create;
    public Transform myTarget;

    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                //playTime = 0.0f;
                MyAnim.SetBool(AnimParam.isMoving, true);
                break;
            case State.Attack:
                worldMoveDir = Vector3.zero;
                myAnim.SetBool(AnimParam.isMoving, false);
                break;
            case State.Death:
                ObjectPoolManager.Instance.ReleaseObj(Data, this.gameObject);
                break;
        }
    }

    float playTime;
    IDamage AttackTarget;
    void StateProcess()
    {
        switch (myState)
        {
            case State.Chase:
                Vector3 dir = transform.forward;
                dir.y = 0;
                SetDirection(dir.normalized);
                break;
            case State.Attack:
                playTime -= Time.deltaTime;
                if(playTime <= 0.0f)
                {
                    if(AttackTarget != null)
                    {
                        //임시
                        if (AttackTarget is Combat combat)
                            if (combat.IsDead) break;
                        if (AttackTarget == null) return;
                        AttackTarget.TakeDamage((short)Data.Ak);
                        myAnim.SetTrigger(AnimParam.Attack);
                    }
                    playTime = CurAttackDelay;
                }
                break;
        }
    }

    void Update()
    {
        //타겟이 없어지면 플레이어를 타겟으로 설정
        if (myTarget == null)
            ResetTarget();

        //타겟을 향해 부드럽게 방향전환
        Vector3 targetDirection = myTarget.position - transform.position;
        targetDirection.y = 0.0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        //
        Physics.Raycast(transform.position, targetDirection, 0.5f);
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
            AttackTarget = collision.gameObject.GetComponent<IDamage>();
            ChangeState(State.Attack);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((attackMask & (1 << collision.gameObject.layer)) != 0)
        {
            ChangeState(State.Chase);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        TakeDamage(MaxHP);
        ChangeState(State.Death);
    }
    #endregion
}
