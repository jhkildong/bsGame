using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class PlayerAction : Player
{
    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }
    Vector2 dir;
    Vector2 inputDir;

    private void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal"); //A, D키의 이동 방향
        dir.y = Input.GetAxisRaw("Vertical"); //W, S키의 이동 방향

        inputDir = Vector2.Lerp(inputDir, dir, Time.deltaTime * 10.0f);

        if (inputDir.sqrMagnitude < 0.001f)
            inputDir = Vector2.zero;

        if(Input.GetMouseButton(0))
        {
            myAnim.SetTrigger(AnimParam.Attack);
        }
    }

    private void FixedUpdate()
    {
        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.y);
    }
}
