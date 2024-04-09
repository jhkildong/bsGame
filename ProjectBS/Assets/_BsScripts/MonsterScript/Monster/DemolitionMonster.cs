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

    protected override void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                break;
            case State.Attack:
                SetDirection(Vector3.zero);
                myAnim.SetBool(AnimParam.isAttacking, true);
                break;
            case State.Death:
                SetDirection(Vector3.zero);
                ObjectPoolManager.Instance.ReleaseObj(this);
                break;
        }
    }
    public void OnAttackPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(myAttackPoint.position, 0.5f, attackMask);
        foreach(var col in colliders)
        {
            IDamage AttackTarget = col.GetComponent<IDamage>();
            AttackTarget?.TakeDamage(Attack);
        }
    }
}
