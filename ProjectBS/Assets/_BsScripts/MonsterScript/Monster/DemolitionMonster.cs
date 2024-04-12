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
        go.AddComponent<AIPerception>().Init(attackMask, FindTarget, LostPlayerInRange);
        animEvent = GetComponentInChildren<AnimEvent>();
        animEvent.AttackAct += OnAttackPoint;
    }

    protected override void ChangeTarget(Transform target)
    {
        base.ChangeTarget(target);
        attackTarget = myTarget.gameObject.GetComponent<IDamage>();
    }

    private void FindTarget(Transform target)
    {
        if(target != PlayerTransform)
            targetQueue.Enqueue(target);
        ChangeState(State.Attack);
    }

    private Transform FindNextTarget()
    {
        while(targetQueue.Count > 0 && targetQueue.Peek() == null)
        {
            targetQueue.Dequeue();
        }
        return targetQueue.Count > 0 ? targetQueue.Peek() : null;
    }

    private void LostPlayerInRange()
    {
        if(targetQueue.Count == 0)
        {
            ChangeState(State.Chase);
        }
    }
    
    protected override void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                Com.MyAnim.SetBool(AnimParam.isAttacking, false);
                attackTarget = null;
                break;
            case State.Attack:
                if (targetQueue.Count > 0)
                    ChangeTarget(targetQueue.Peek());
                else
                    ChangeTarget(PlayerTransform);
                break;
            case State.Death:
                SetDirection(Vector3.zero);
                ObjectPoolManager.Instance.ReleaseObj(this);
                break;
        }
    }

    protected override void StateProcess()
    {
        switch (myState)
        {
            case State.Chase:
                SetDirection(transform.forward);
                break;
            case State.Attack:
                if(targetQueue.Count > 0)
                {
                    if (targetQueue.Peek() == null)
                    {
                        Transform nextTarget = FindNextTarget();
                        if (nextTarget == null)
                            ChangeTarget(PlayerTransform);
                        else
                            ChangeTarget(nextTarget);
                    }
                }
                Vector3 dir = myTarget.position - transform.position;
                if(dir.sqrMagnitude > DemolitionData.AttackRange * DemolitionData.AttackRange)
                {
                    SetDirection(transform.forward);
                    Com.MyAnim.SetBool(AnimParam.isAttacking, false);
                }
                else
                {
                    SetDirection(Vector3.zero);
                    Com.MyAnim.SetBool(AnimParam.isAttacking, true);
                }
                break;
            case State.Death:
                break;
        }
    }
    
    public void OnAttackPoint()
    {
        Vector3 dir = myTarget.position - transform.position;
        if (dir.sqrMagnitude < DemolitionData.AttackRange * DemolitionData.AttackRange)
        {
            float Damage = Attack;
            if (attackTarget is Building) Damage += DemolitionData.BuildingDmg;
            attackTarget?.TakeDamage((short)Damage);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {

    }

    protected override void OnCollisionExit(Collision collision)
    {

    }
}
