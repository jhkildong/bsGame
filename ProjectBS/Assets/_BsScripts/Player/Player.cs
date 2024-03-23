using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using Yeon;

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
    /// <summary>
    /// 플레이어의 키 조작을 버튼 형식으로 받음
    /// 상하, 좌우를 동시에 입력시 나중에 입력된 동작으로 덮어씌워짐
    /// 버튼을 떼어낼 때 비트 연산을 통해 이전 입력이 있었는지 확인한 후 입력값 설정
    /// </summary>
    #region PlayerInput
    
    private PlayerInputs playerInputs;
    
    private void OnAttack(InputAction.CallbackContext context)
    {
        myAnim.SetTrigger(AnimParam.Attack);
    }

    #region WSAD switch

    byte WSInput = 0b_00;
    byte ADInput = 0b_00;
    byte W = 0b_01, A = 0b_01;
    byte S = 0b_10, D = 0b_10;

    private void PressW(InputAction.CallbackContext context)
    {
        moveDir.z = 1.0f;
        WSInput |= W;
    }

    private void PressS(InputAction.CallbackContext context)
    {
        moveDir.z = -1.0f;
        WSInput |= S;
    }

    private void PressA(InputAction.CallbackContext context)
    {
        moveDir.x = -1.0f;
        ADInput |= A;
    }

    private void PressD(InputAction.CallbackContext context)
    {
        moveDir.x = 1.0f;
        ADInput |= D;
    }

    private void ReleaseW(InputAction.CallbackContext context)
    {
        WSInput ^= W;
        moveDir.z = WSInput == S ? -1.0f : 0f;
    }

    private void ReleaseS(InputAction.CallbackContext context)
    {
        WSInput ^= S;
        moveDir.z = WSInput == W ? 1.0f : 0f;
    }

    private void ReleaseA(InputAction.CallbackContext context)
    {
        ADInput ^= A;
        moveDir.x = ADInput == D ? 1.0f : 0f;
    }

    private void ReleaseD(InputAction.CallbackContext context)
    {
        ADInput ^= D;
        moveDir.x = ADInput == A ? -1.0f : 0f;
    }

    private void SetAnimMove(InputAction.CallbackContext context)
    {
        myAnim.SetBool(AnimParam.isMoving, true);
    }

    private void SetAnimStop(InputAction.CallbackContext context)
    {
        if ((WSInput | ADInput) != 0) return;
        myAnim.SetBool(AnimParam.isMoving, false);
    }
    #endregion

    #endregion

    Rig[] myRigs;

    private void InitPlayerSetting()
    {
        myRigs = GetComponentsInChildren<Rig>();
        playerInputs = new PlayerInputs();
        ChangeHpAct += PlayerUI.Instance.ChangeHP;
        DeadAct += Death;
        CurHp = MaxHP;
        attackMask = (int)BSLayerMasks.Monster;


        #region PlayerInputsCallback Setting
        playerInputs.Player.Attack.performed += OnAttack;
        playerInputs.Player.PressW.performed += PressW;
        playerInputs.Player.PressS.performed += PressS;
        playerInputs.Player.PressA.performed += PressA;
        playerInputs.Player.PressD.performed += PressD;
        playerInputs.Player.ReleaseW.performed += ReleaseW;
        playerInputs.Player.ReleaseS.performed += ReleaseS;
        playerInputs.Player.ReleaseA.performed += ReleaseA;
        playerInputs.Player.ReleaseD.performed += ReleaseD;
        playerInputs.Player.MoveAnim.started += SetAnimMove;
        playerInputs.Player.MoveAnim.canceled += SetAnimStop;
        #endregion
        playerInputs.Enable();
    }

    private void OnDestroy()
    {
        playerInputs.RemoveAllBindingOverrides();
        playerInputs.Dispose();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }


    void Death()
    {
        myAnim.SetTrigger(AnimParam.Death);
        isOutOfControl = true;
        foreach(Rig rig in myRigs)
        {
            rig.weight = 0;
        }
        playerInputs.Disable();
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
        //죽은 상태면 return
        if (IsDead())
            return;
        //입력이 없는 상태면 return
        if((WSInput | ADInput) == 0b_00)
            return;
        
        moveDir.Normalize();
        //바라보는 방향기준의 애니메이션 방향(입력받은 방향에서 바라보는 방향의 반대방향으로 회전)
        dir = Quaternion.AngleAxis(-myCharacter.rotation.eulerAngles.y, Vector3.up) * moveDir;

        inputDir = Vector3.Lerp(inputDir, dir, Time.deltaTime * 10.0f);

        if (inputDir.magnitude < 0.01f)
        {
            inputDir = Vector3.zero;
        }
    }

    protected override void FixedUpdate()
    {
        worldMoveDir = moveDir;
        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.z);
        base.FixedUpdate();
    }

    
}
