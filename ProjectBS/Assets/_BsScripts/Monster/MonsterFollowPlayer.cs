using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollowPlayer : Yeon.Movement
{
    public Transform target;
    Monster myMonster;
    Vector3 dir;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //임시처리 TODO 플레이어 스크립트에 static으로 transform값을 받아오게 변경
        if (target == null)
        {
            target = GameObject.Find("Player").transform;
        }
        TryGetComponent(out myMonster);
        moveSpeed = myMonster.Speed;
    }

    private void Update()
    {
        dir = target.position - transform.position;
        if (dir.magnitude > 1)
            dir.Normalize();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        worldMoveDir = dir;
        base.FixedUpdate();
    }

    public void block(bool state)
    {
        isBlocked = state;
    }
}

