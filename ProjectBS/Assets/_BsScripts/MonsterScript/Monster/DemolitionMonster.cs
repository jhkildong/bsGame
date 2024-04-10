using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolitionMonster : GroupMonster
{
    public DemolitionMonsterData DemolitionData => _data as DemolitionMonsterData;

    private AnimEvent animEvent;

    public override void Init(MonsterData data)
    {
        base.Init(data);
        GameObject go = new GameObject("AIPerception");
        go.transform.SetParent(this.transform);
        go.AddComponent<AIPerception>().Init(attackMask, ChangeTarget, ResetTarget);
        animEvent = GetComponentInChildren<AnimEvent>();
        myAttackPoint = transform.Find("AttackPoint");
        animEvent.AttackAct += OnAttackPoint;
    }

    //AIperception에서 범위내에 적이 들어 왔을 때 공격 상태로 전환
    protected override void ChangeTarget(Transform target)
    {
        base.ChangeTarget(target);
        ChangeState(State.Attack);
    }

    //적이 사라졌을 때 추적 상태로 전환
    protected override void ResetTarget()
    {
        base.ResetTarget();
        ChangeState(State.Chase);
    }
    
    protected override void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                Com.MyAnim.SetBool(AnimParam.isAttacking, false);
                break;
            case State.Attack:
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
        Collider[] colliders = Physics.OverlapSphere(Com.MyAttackPoint.position, 0.5f, (int)attackMask);
        if(colliders.Length == 0)
            return;
        foreach(var col in colliders)
        {
            IDamage AttackTarget = col.GetComponent<IDamage>();
            float Damage = Attack;
            if (AttackTarget is Building) Damage += DemolitionData.BuildingDmg;
            AttackTarget?.TakeDamage((short)Damage);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {

    }
}
