using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : Yeon.Movement
{
    public Transform target;
    Vector3 dir;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (target == null)
        {
            target = GameObject.Find("Player").transform;
        }    
    }

    private void Update()
    {
        dir = target.position - transform.position;
        if(dir.magnitude > 1)
            dir.Normalize();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        worldMoveDir = dir;
        base.FixedUpdate();
    }
}
