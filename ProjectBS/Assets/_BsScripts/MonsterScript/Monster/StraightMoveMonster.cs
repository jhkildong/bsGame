using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StraightMoveMonster : GroupMonster
{
    public StraightMoveMonsterData StraightData => _data as StraightMoveMonsterData;
    Transform target;

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
    }

    protected override void InitCollider()
    {
        SphereCollider sphere;
        sphere = gameObject.AddComponent<SphereCollider>();
        sphere.radius = 1.5f;
        sphere.center = new Vector3(0, 1.8f, 0);
        sphere.isTrigger = true;
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