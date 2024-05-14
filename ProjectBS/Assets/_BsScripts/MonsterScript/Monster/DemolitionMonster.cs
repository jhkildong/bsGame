using System.Collections.Generic;
using UnityEngine;

public class DemolitionMonster : GroupMonster
{
    public DemolitionMonsterData DemolitionData => _data as DemolitionMonsterData;

    private AnimEvent animEvent;
    private bool isPlayerInRange = false;
    private Queue<Transform> targetQueue = new Queue<Transform>();

    public override void Init(MonsterData data)
    {
        base.Init(data);
        GameObject go = new GameObject("AIPerception");
        go.transform.SetParent(transform);
        go.AddComponent<AIPerception>().Init(attackMask, FindTarget, LostPlayer);
        animEvent = GetComponentInChildren<AnimEvent>();
        animEvent.AttackAct += OnAttackPoint;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Data == null)
            return;
        Com.MyAnim.SetBool(AnimParam.Wait, false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Com.MyAnim.SetBool(AnimParam.Wait, true);
    }

    private void ChangeTarget(Transform target)
    {
        if (target == null)
            return;
        myTarget = target;
    }

    private void FindTarget(Transform target)
    {
        if (target != PlayerTransform)
        {
            targetQueue.Enqueue(target);
        }
        else
        {
            isPlayerInRange = true;
        }
    }

    private void LostPlayer(Transform target)
    {
        if (target == PlayerTransform)
        {
            isPlayerInRange = false;
        }
    }

    private void CheckTarget()
    {
        while (targetQueue.Count > 0 && targetQueue.Peek() == null)
        {
            targetQueue.Dequeue();
        }
        if (targetQueue.Count > 0)
        {
            ChangeTarget(targetQueue.Peek());
        }
        else
        {
            ChangeTarget(PlayerTransform);
        }
    }


    #region BehaviorTree

    protected override INode SettingBT()
    {
        return new SelectorNode(
            new List<INode>()
            {
                new ActionNode(CheckDeath),
                new SequenceNode (
                     new List<INode>()
                     {
                         new ActionNode(CheckTargetInRange),
                         new ActionNode(CheckTargetInAttackRange),
                         new ActionNode(CheckAttackTime),
                         new ActionNode(AttackTarget)
                     }),
                new ActionNode(MoveToTarget)
            }
        );
    }

    private INode.NodeState CheckTargetInRange()
    {
        SetDirection(transform.forward);
        CheckTarget();
        if (targetQueue.Count > 0 || isPlayerInRange)
            return INode.NodeState.Success;
        else
            return INode.NodeState.Failure;
    }

    protected override INode.NodeState CheckTargetInAttackRange()
    {
        if (Vector3.SqrMagnitude(myTarget.position - transform.position) <= DemolitionData.AttackRange * DemolitionData.AttackRange)
        {
            SetDirection(Vector3.zero);
            attackTarget = myTarget.GetComponent<IDamage>();
            return INode.NodeState.Success;
        }
        else
        {
            return INode.NodeState.Running;
        }
    }

    protected override INode.NodeState CheckAttackTime()
    {
        if (Com.MyAnim.GetCurrentAnimatorStateInfo(0).shortNameHash == AnimParam.Attack)
        {
            return INode.NodeState.Running;
        }
        else
        {
            return INode.NodeState.Success;
        }
    }

    protected override INode.NodeState AttackTarget()
    {
        if (myTarget != null)
        {
            Com.MyAnim.SetTrigger(AnimParam.Attack);
            return INode.NodeState.Success;
        }
        else
        {
            SetDirection(transform.forward);
            return INode.NodeState.Failure;
        }
    }

    protected override INode.NodeState MoveToTarget()
    {
        if (myTarget == null)
            myTarget = PlayerTransform;
        SetDirection(transform.forward);
        return INode.NodeState.Success;
    }


    #endregion

    public void OnAttackPoint()
    {
        Vector3 dir = myTarget.position - transform.position;
        if (dir.magnitude <= DemolitionData.AttackRange)
        {
            float Damage = Attack;
            if (attackTarget is Building) Damage += DemolitionData.BuildingDmg;
            attackTarget?.TakeDamageEffect(Damage);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {

    }

    protected override void OnCollisionExit(Collision collision)
    {

    }
}
