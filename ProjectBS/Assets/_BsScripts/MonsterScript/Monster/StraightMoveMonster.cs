using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMoveMonster : GroupMonster
{
    public StraightMoveMonsterData StraightData => _data as StraightMoveMonsterData;
    Transform target;
    Coroutine releaseCoroutine;

    protected override void Awake()
    {
        base.Awake();
        rBody.useGravity = false;
    }

    public override void Init(MonsterData data)
    {
        base.Init(data as StraightMoveMonsterData);
        GameObject go = new GameObject("myTarget");
        go.transform.SetParent(this.transform);
        go.transform.localPosition = new Vector3(0, 0, 1);
        target = go.transform;
        myTarget = target;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(StraightData == null)
        {
            return;
        }
        myTarget = target.transform;
        releaseCoroutine = StartCoroutine(ReleaseAuto());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        releaseCoroutine = null;
    }

    protected override void InitCollider()
    {
        base.InitCollider();
        col.isTrigger = true;
    }

    IEnumerator ReleaseAuto()
    {
        yield return new WaitForSeconds(10.0f);
        ObjectPoolManager.Instance.ReleaseObj(this);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if ((attackMask & (1 << other.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = other.gameObject.GetComponent<IDamage>();
            AttackTarget.TakeDamage(Attack);
            //ChangeState(State.Attack);
        }
    }

}