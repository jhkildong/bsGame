using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class Test : Movement
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    Vector3 dir = new();

    private void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal"); //A, D키의 이동 방향
        dir.z = Input.GetAxisRaw("Vertical"); //W, S키의 이동 방향
        dir.Normalize();
    }

    protected override void FixedUpdate()
    {
        worldMoveDir = dir;
        base.FixedUpdate();
    }
}
