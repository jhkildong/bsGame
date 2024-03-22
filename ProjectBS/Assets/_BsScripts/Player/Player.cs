using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Yeon;
using static UnityEngine.InputSystem.InputAction;

public enum BSLayerMasks
{
    Player = 1 << 14,
    Monster = 1 << 15,
    Building = 1 << 24,
    InCompletedBuilding = 1 << 25,
    BuildCheckObject = 1 << 26,
    Ground = 1 << 29
}

public class Player : Combat
{
    CallbackContext WSaction;
    CallbackContext ADaction;
    private void InitPlayerSetting()
    {
        ChangeHpAct += PlayerUI.Instance.ChangeHP;
        DeadAct += Death;
        CurHp = MaxHP;
        attackMask = (int)BSLayerMasks.Monster;
    }

    void Death()
    {
        myAnim.SetTrigger(AnimParam.Death);
        GetComponent<PlayerInput>().actions.Disable();
    }

    protected override void Start()
    {
        base.Start();
        InitPlayerSetting();
    }

    Vector3 moveDir;
    Vector3 dir;
    Vector3 inputDir;
    public Transform myCharacter;

    private void Update()
    {
        if (IsDead())
            return;
        //�ٶ󺸴� ��������� �ִϸ��̼� ����(�Է¹��� ���⿡�� �ٶ󺸴� ������ �ݴ�������� ȸ��)
        dir = Quaternion.AngleAxis(-myCharacter.rotation.eulerAngles.y, Vector3.up) * moveDir;

        inputDir = Vector3.Lerp(inputDir, dir, Time.deltaTime * 10.0f);
        inputDir.x = Mathf.Clamp(inputDir.x, -1.0f, 1.0f);
        inputDir.z = Mathf.Clamp(inputDir.z, -1.0f, 1.0f);

        if (inputDir.magnitude < 0.01f)
        {
            inputDir = Vector3.zero;
        }
    }

    protected override void FixedUpdate()
    {
        worldMoveDir = moveDir.normalized;
        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.z);
        base.FixedUpdate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input != null)
        {
            myAnim.SetBool("isMoving", true);
            moveDir = new(input.x, 0f, input.y);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            myAnim.SetTrigger(AnimParam.Attack);
        }
    }

    public void PressW(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDir.z = 1.0f;
        }
    }

    public void PressS(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDir.z = -1.0f;
        }
    }

    public void PressA(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDir.x = -1.0f;
        }
    }

    public void PressD(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDir.x = 1.0f;
        }
    }
}
