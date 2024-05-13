using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeon;

public abstract class Monster : Combat, IDamage<Monster>, IPoolable
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
    protected Transform PlayerTransform
    {
        get
        {
            if(_playerTransform == null)
            {
                _playerTransform = GameManager.Instance.Player.transform;
            }
            return _playerTransform;
        }
    }
    #endregion

    #region Private Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] protected MonsterComponent _monsterComponent;
    [SerializeField] protected MonsterData _data;
    private Transform _playerTransform;
    #endregion

    #region Interface Method
    ////////////////////////////////InterfaceMethod////////////////////////////////

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (CurHp <= 0)
        {
            DeadTransformAct?.Invoke(this.transform);
        }
    }

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

        _maxHp = data.MaxHP;
        _attack = data.Ak;

        _curHp = data.MaxHP;
        moveSpeed = data.Sp;

        attackMask = (int)(BSLayerMasks.Player | BSLayerMasks.Building);
        gameObject.layer = (int)BSLayers.Monster;

        rBody.mass = data.Mass;
        rBody.angularDrag = 2f;
        InitCollider(data.Radius);  //data의 radius값으로 콜라이더 설정

        _monsterComponent = GetComponentInChildren<MonsterComponent>();

        //타격 이펙트 설정
        effectData.effectCount = 1;
        effectData.effectColor = Color.white;
        effectData.effectTime = 0.1f;
        effectData.SetRenderer(_monsterComponent.Myrenderers);

        DeadAct += Die;

        #region BuffChangeAct Setting
        _buff.asBuffDict.ChangeBuffAct += () => { Com.MyAnim.SetFloat(AnimParam.AttackSpeed, Aksp); };
        _buff.hpBuffDict.ChangeBuffAct += () =>
        {
            float changeHp = MaxHp - tempMaxHp;
            if (CurHp + changeHp > 0)
                CurHp += changeHp;
            else
                CurHp = 1;
            tempMaxHp = MaxHp;
        };
        _buff.msBuffDict.ChangeBuffAct += () => { moveSpeedBuff = 1 + getBuff.msBuff; };
        #endregion
    }

    protected override void InitCollider(float radius)
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
        _BTRunner = new BehaviorTreeRunner(SettingBT());
    }

    //활성화 될 때 상태 초기화
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Data == null) return;

        _curHp = Data.MaxHP;
        moveSpeed = Data.Sp;
    }

    #endregion

    #region Private Method
    ////////////////////////////////PrivateMethod////////////////////////////////
    private void Die()
    {
        Com.MyAnim.SetTrigger(AnimParam.Death);
        GameObject go = ItemManager.Instance.DropRandomItem(Data.DropItemList);
        if (go != null)
            go.transform.position = transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-1, 1), 0 , Random.Range(-1, 1));
        GameObject exp = ItemManager.Instance.DropExp(Data.Exp);
        if(exp != null)
            exp.transform.position = transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        GameObject gold = ItemManager.Instance.DropGold(Data.Gold);
        if(gold != null)
            gold.transform.position = transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
    }
    #endregion

 
    #region BehaviorTree
    private BehaviorTreeRunner _BTRunner;
    private float playTime = 0.0f;
    protected Transform myTarget;
    protected IDamage attackTarget;

    protected virtual INode SettingBT()
    {
        return new SelectorNode(
            new List<INode>()
            {
                new ActionNode(CheckDeath),
                new SequenceNode (
                     new List<INode>()
                     {
                         new ActionNode(CheckTargetInAttackRange),
                         new ActionNode(CheckAttackTime),
                         new ActionNode(AttackTarget)
                     }),
                new ActionNode(MoveToTarget)
            }
        );
    }

    protected virtual INode.NodeState CheckDeath()
    {
        if (IsDead)
        {
            Die();
            ObjectPoolManager.Instance.ReleaseObj(this);
            return INode.NodeState.Success;
        }
        else
            return INode.NodeState.Failure;
    }

    protected virtual INode.NodeState CheckTargetInAttackRange()
    {
        if (attackTarget != null)
        {
            SetDirection(Vector3.zero);
            return INode.NodeState.Success;
        }
        else
        {
            SetDirection(transform.forward);
            return INode.NodeState.Failure;
        }
    }

    protected virtual INode.NodeState CheckAttackTime()
    {
        if (playTime <= 0.0f)
        {
            playTime = AttackDelay;
            return INode.NodeState.Success;
        }
        else
        {
            playTime -= Time.deltaTime;
            return INode.NodeState.Running;
        }
    }

    protected virtual INode.NodeState AttackTarget()
    {
        if (attackTarget != null)
        {
            attackTarget.TakeDamageEffect(Attack);
            return INode.NodeState.Success;
        }
        else
        {
            SetDirection(transform.forward);
            return INode.NodeState.Failure;
        }
    }

    protected virtual INode.NodeState MoveToTarget()
    {
        if (myTarget == null)
        {
            SetDirection(Vector3.zero);
            return INode.NodeState.Failure;
        }
        return INode.NodeState.Success;
    }
    #endregion

    protected virtual void Update()
    {
        if (myTarget == null)
            myTarget = PlayerTransform;
        _BTRunner.Operate();

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

    #region Collision Event
    ////////////////////////////////CollisionEvent////////////////////////////////
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & attackMask) != 0)
        {
            if (Vector3.Dot(collision.transform.position - transform.position, transform.forward) > 0)
            {
                IDamage target = collision.gameObject.GetComponent<IDamage>();
                if (target != null)
                {
                    attackTarget = target;
                }
            }
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & attackMask) != 0)
        {
            if (attackTarget == collision.gameObject.GetComponent<IDamage>())
            {
                attackTarget = null;
            }
        }
    }
    #endregion
}
