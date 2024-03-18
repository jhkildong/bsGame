using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Yeon;

public class PlayerAction : Player
{
    Vector2 moveVal;
    Vector3 moveDir;
    Vector3 dir;
    Vector3 inputDir;
    public float angle;
    public LayerMask building;
    private void Update()
    {
        //현재 월드 기준 바라보는 방향의 각도
        angle = transform.rotation.eulerAngles.y;
        //바라보는 방향기준의 애니메이션 방향(입력받은 방향에서 바라보는 방향의 반대방향으로 회전)
        dir = Quaternion.AngleAxis(-angle , Vector3.up) * moveDir;

        inputDir = Vector3.Lerp(inputDir, dir, Time.deltaTime * 8.0f);
        inputDir.x = Mathf.Clamp(inputDir.x, -1.0f, 1.0f);
        inputDir.z = Mathf.Clamp(inputDir.z, -1.0f, 1.0f);

        if (inputDir.sqrMagnitude < 0.001f)
        {
            myAnim.SetBool("isMoving", false);
            inputDir = Vector3.zero;
        }
        else
            myAnim.SetBool("isMoving", true);
    }

    protected override void FixedUpdate()
    {
        SetDirection(moveDir.normalized);
        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.z);
        base.FixedUpdate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null)
        {
            moveDir = new(input.x, 0f, input.y);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            myAnim.SetTrigger(AnimParam.Attack);
        }
    }
}
