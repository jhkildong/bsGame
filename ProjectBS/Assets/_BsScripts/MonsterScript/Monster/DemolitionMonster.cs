using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolitionMonster : GroupMonster
{
    public DemolitionMonsterData DemolitionData => _data as DemolitionMonsterData;

    private AnimEvent animEvent;
    private Queue<Transform> targetQueue = new Queue<Transform>();

    public override void Init(MonsterData data)
    {
        base.Init(data);
        GameObject go = new GameObject("AIPerception");
        go.transform.SetParent(this.transform);
        go.AddComponent<AIPerception>().Init(attackMask, FindTarget);
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

    protected void ChangeTarget(Transform target)
    {
        if (myTarget != null) 
            attackTarget = myTarget.gameObject.GetComponent<IDamage>();
        else
            attackTarget = null;
    }

    private void FindTarget(Transform target)
    {
        if(target != PlayerTransform)
        {
            targetQueue.Enqueue(target);
            Transform tr = targetQueue.Peek();
            ChangeTarget(tr);
        }
    }

    private Transform FindNextTarget()
    {
        while(targetQueue.Count > 0 && targetQueue.Peek() == null)
        {
            targetQueue.Dequeue();
        }
        return targetQueue.Count > 0 ? targetQueue.Peek() : null;
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
                         new ActionNode(CheckTargetInAttackRange),
                         new ActionNode(CheckAttackTime),
                         new ActionNode(AttackTarget)
                     }),
                new ActionNode(MoveToTarget)
            }
        );
    }

    protected override INode.NodeState CheckTargetInAttackRange()
    {
        myTarget = FindNextTarget();
        if (attackTarget != null && (Vector3.SqrMagnitude(myTarget.position - transform.position) <= DemolitionData.AttackRange))
            return INode.NodeState.Success;
        else
            return INode.NodeState.Failure;
    }

    protected override INode.NodeState AttackTarget()
    {
        if (myTarget != null)
        {
            Com.MyAnim.SetTrigger(AnimParam.Attack);
            return INode.NodeState.Success;
        }
        return INode.NodeState.Failure;
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
