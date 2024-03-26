using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using Yeon;

[System.Flags]
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
    /// �÷��̾��� Ű ������ ��ư �������� ����
    /// ����, �¿츦 ���ÿ� �Է½� ���߿� �Էµ� �������� �������
    /// ��ư�� ��� �� ��Ʈ ������ ���� ���� �Է��� �־����� Ȯ���� �� �Է°� ����
    /// </summary>
    #region PlayerInput
    private PlayerInputs playerInputs;
    
    private void OnAttack(InputAction.CallbackContext context)
    {
        MyAnim.SetTrigger(AnimParam.Attack);
    }

    #region WSAD switch

    byte WSInput = 0b_00;
    byte ADInput = 0b_00;
    private readonly byte W = 0b_01, A = 0b_01;
    private readonly byte S = 0b_10, D = 0b_10;

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
        MyAnim.SetBool(AnimParam.isMoving, true);
    }
    private void SetAnimStop(InputAction.CallbackContext context)
    {
        if ((WSInput | ADInput) != 0) return;
        MyAnim.SetBool(AnimParam.isMoving, false);
    }
    #endregion

    #endregion

    #region Private Field
    private Rig[] myRigs;
    private ParticleSystem myParticle;
    #endregion


    private void InitPlayerSetting()
    {
        myRigs = GetComponentsInChildren<Rig>();
        ChangeHpAct += PlayerUI.Instance.ChangeHP;
        DeadAct += Death;
        CurHp = MaxHP;
        attackMask = (int)BSLayerMasks.Monster;
        #region PlayerInputsCallback Setting
        playerInputs = new PlayerInputs();
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
        playerInputs.Enable();
        #endregion
    }


    void Death()
    {
        MyAnim.SetTrigger(AnimParam.Death);
        isOutOfControl = true;
        foreach(Rig rig in myRigs)
        {
            rig.weight = 0;
        }
        playerInputs.Disable();
    }

    protected override void Awake()
    {
        base.Awake();
        InitPlayerSetting();
    }

    Vector3 moveDir;
    Vector3 dir;
    Vector3 inputDir;
    public Transform myCharacter;

    private void Update()
    {
        //���� ���¸� return
        if (IsDead)
            return;
        //�Է��� ���� ���¸� return
        if((WSInput | ADInput) == 0b_00)
            return;
        
        moveDir.Normalize();
        //�ٶ󺸴� ��������� �ִϸ��̼� ����(�Է¹��� ���⿡�� �ٶ󺸴� ������ �ݴ�������� ȸ��)
        dir = Quaternion.AngleAxis(-myCharacter.rotation.eulerAngles.y, Vector3.up) * moveDir;

        inputDir = Vector3.Lerp(inputDir, dir, Time.deltaTime * 10.0f);

        if (inputDir.magnitude < 0.01f)
        {
            inputDir = Vector3.zero;
        }
    }

    public void OnAttackPoint()
    {
        Collider[] list = Physics.OverlapSphere(myAttackPoint.position, 1.0f, attackMask);
        myParticle.transform.parent.rotation = myCharacter.rotation;
        myParticle.Play();

        foreach (Collider col in list)
        {
            IDamage<Combat> obj = col.GetComponent<IDamage<Combat>>();
            if (obj != null)
            {
                obj.TakeDamage(Attack);
            }
        }
    }

    protected override void FixedUpdate()
    {
        worldMoveDir = moveDir;
        MyAnim.SetFloat("x", inputDir.x);
        MyAnim.SetFloat("y", inputDir.z);
        base.FixedUpdate();
    }
}
