using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolitionMonster : GroupMonster
{
    public DemolitionMonsterData DemolitionData => _data as DemolitionMonsterData;

    private Queue<Transform> targetQueue = new Queue<Transform>();

    public override void AttackTarget()
    {
        if (attackTarget == null) return;

        Com.MyAnim.SetTrigger(AnimParam.Attack);
    }

    public override bool AttackReady()
    {
        return attackReady;
    }

    public override bool AttackTargetInRange()
    {
        if (myTarget == null)
        {
            return false;
        }
        if (Vector3.SqrMagnitude(myTarget.position - transform.position) < DemolitionData.AttackRange * DemolitionData.AttackRange)
        {
            attackTarget = myTarget.GetComponent<IDamage>();
        }
        else
        {
            attackTarget = null;
        }
        return base.AttackTargetInRange();
    }


    public override void Init(MonsterData data)
    {
        base.Init(data);
        GameObject go = new GameObject("AIPerception");
        go.transform.SetParent(this.transform);
        go.AddComponent<AIPerception>().Init(attackMask, FindTarget);
        Com.MyAnimEvent.AttackAct += OnAttackPoint;
        Com.MyAnim.GetBehaviour<MonsterAttack>().monster = this;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Data == null)
            return;
        Com.MyAnim.SetBool(AnimParam.isMoving, true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Com.MyAnim.SetBool(AnimParam.isMoving, false);
    }


    private void FindTarget(Transform target)
    {
        if(target != PlayerTransform)
        {
            targetQueue.Enqueue(target);
            Transform tr = targetQueue.Peek();
            myTarget = tr;
        }
    }

    private void FindNextTarget()
    {
        while(targetQueue.Count > 0 && targetQueue.Peek() == null)
        {
            targetQueue.Dequeue();
        }
        if (targetQueue.Count > 0)
        {
            myTarget = targetQueue.Peek();
        }
        else
        {
            myTarget = PlayerTransform;
        }
    }

    protected override void Update()
    {
        if (myTarget == null)
            FindNextTarget();
        base.Update();
    }
    
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
        //nothing
    }

    protected override void OnCollisionExit(Collision collision)
    {
        //nothing
    }
    
}
