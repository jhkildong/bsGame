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
        dir.x = Input.GetAxisRaw("Horizontal"); //A, DŰ�� �̵� ����
        dir.z = Input.GetAxisRaw("Vertical"); //W, SŰ�� �̵� ����
        dir.Normalize();
    }

    protected override void FixedUpdate()
    {
        worldMoveDir = dir;
        base.FixedUpdate();
    }
}
