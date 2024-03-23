using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Yeon;

[RequireComponent(typeof(DropTable))]
public abstract class Monster : Combat, IDropable
{
    Transform PlayerTransform;

    public event UnityAction<Transform> DeadTransformAct;
    [SerializeField] private MonsterData _data;
    public MonsterData Data => _data;
    public List<dropItem> dropItems() => Data.DropItemList;
    public void WillDrop()
    {
        //GameObject go = ItemManager.Instance.A(dropItems());
        //go.transform.position = this.transform.position;
    }
   
    /// <summary>몬스터 공격딜레이</summary>
    public float AttackDelay => Data.AkDelay;

    [SerializeField]private float _curAttackDelay;
    public float CurAttackDelay{ get => _curAttackDelay; set => _curAttackDelay = value; }
    public bool isAttack => CurAttackDelay < AttackDelay;

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
    public override void TakeDamage(short damage)
    {
        base.TakeDamage(damage);
        if(CurHp <=0)
        {
            DeadTransformAct?.Invoke(this.transform);
        }
    }


    protected override void Start()
    {
        base.Start();
        ChangeState(State.Chase);
        dropTable = GetComponent<DropTable>();
    }

    protected virtual void OnEnable()
    {
        if (Data == null) return;
        moveSpeed = Data.Sp;
        CurHp = Data.MaxHP;
        if(myAnim == null) myAnim = GetComponentInChildren<Animator>();
        ChangeState(State.Chase);
    }

    private DropTable dropTable;
    void Death()
    {
        myAnim.SetBool(AnimParam.isMoving, false);
        myAnim.SetTrigger(AnimParam.Death);
        ChangeState(State.Death);
        dropTable.WillDrop(dropItems()).transform.position = this.transform.position;
    }

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
                playTime = 0.0f;
                myAnim.SetBool(AnimParam.isMoving, true);
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
                Vector3 dir = myTarget.position - transform.position;
                dir.Normalize();
                worldMoveDir = dir;
                break;
            case State.Attack:
                playTime -= Time.deltaTime;
                if(playTime <= 0.0f)
                {
                    if(AttackTarget != null)
                    {
                        //임시
                        if (AttackTarget is Combat combat)
                            if (combat.IsDead()) break;
                        if (AttackTarget == null) return;
                        AttackTarget.TakeDamage((short)Data.Ak);
                        myAnim.SetTrigger(AnimParam.Attack);
                    }
                    playTime = AttackDelay;
                }
                break;
        }
    }

    IEnumerator DelayChangeState(State s, float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(s);
    }

    // Update is called once per frame
    void Update()
    {
        if(myTarget == null)
            ResetTarget();
        Vector3 targetDirection = myTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        StateProcess();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void ChangeTarget(Transform target)
    {
        if (myState == State.Death) return;
        myTarget = target;
        ChangeState(State.Chase);
        
    }
    
    public void ResetTarget()
    {
        if (myState == State.Death) return;
        myTarget = PlayerTransform;
    }

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
        Destroy(gameObject);
    }

    #endregion

}
