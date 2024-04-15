using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Yeon;

public enum AttackState
{
    None,
    Attack,
    ComboCheck
}

public class Player : Combat, IDamage<Player>
{

    #region PlayerInput
    ////////////////////////////////PlayerInput////////////////////////////////
    // �÷��̾��� Ű ������ ��ư �������� ����
    // ����, �¿츦 ���ÿ� �Է½� ���߿� �Էµ� �������� �������
    // ��ư�� ��� �� ��Ʈ ������ ���� ���� �Է��� �־����� Ȯ���� �� �Է°� ����
    ///////////////////////////////////////////////////////////////////////////

    private PlayerInputs playerInputs;
    private void OnAttack(InputAction.CallbackContext context)
    {
        switch (attackState)
        {
            case AttackState.None:
                Com.MyAnim.SetTrigger(AnimParam.Attack);
                break;
            case AttackState.Attack:
                Com.MyAnim.SetBool(AnimParam.isAttacking, true);
                break;
            case AttackState.ComboCheck:
                Com.MyAnim.SetBool(AnimParam.isComboReady, true);
                break;
        }
    }

    #region WSAD switch

    [SerializeField] byte WSADInput = 0b_0000;
    private readonly byte W = 0b_0001, S = 0b_0010, A = 0b_0100, D = 0b_1000;

    private void PressW(InputAction.CallbackContext context)
    {
        _moveDir.z = 1.0f;
        WSADInput |= W; //W�� �����ٰ� ǥ��
    }
    private void PressS(InputAction.CallbackContext context)
    {
        _moveDir.z = -1.0f;
        WSADInput |= S; //S�� �����ٰ� ǥ��
    }
    private void PressA(InputAction.CallbackContext context)
    {
        _moveDir.x = -1.0f;
        WSADInput |= A; //A�� �����ٰ� ǥ��
    }
    private void PressD(InputAction.CallbackContext context)
    {
        _moveDir.x = 1.0f;
        WSADInput |= D; //D�� �����ٰ� ǥ��
    }
    private void ReleaseW(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~W; //W�� �����ٰ� ǥ��
        _moveDir.z = (WSADInput & S) != 0 ? -1.0f : 0f; //S�� ���������� -1, �ƴϸ� 0
    }
    private void ReleaseS(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~S; //S�� �����ٰ� ǥ��
        _moveDir.z = (WSADInput & W) != 0 ? 1.0f : 0f; //W�� ���������� 1, �ƴϸ� 0
    }
    private void ReleaseA(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~A;  //A�� �����ٰ� ǥ��
        _moveDir.x = (WSADInput & D) != 0 ? 1.0f : 0f; //D�� ���������� 1, �ƴϸ� 0
    }
    private void ReleaseD(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~D;  //D�� �����ٰ� ǥ��
        _moveDir.x = (WSADInput & A) != 0 ? -1.0f : 0f; //A�� ���������� -1, �ƴϸ� 0
    }
    private void SetAnimMove(InputAction.CallbackContext context)
    {
        Com.MyAnim.SetBool(AnimParam.isMoving, true); //WSAD �� �ϳ��� ���������� �̵���
    }
    private void SetAnimStop(InputAction.CallbackContext context)
    {
        if (WSADInput != 0b_0000)  //WSAD �� �ϳ��� ���������� ����
            return;
        Com.MyAnim.SetBool(AnimParam.isMoving, false);
    }
    #endregion

    #endregion

    #region Private Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] private PlayerComponent Com;
    [SerializeField] private AttackState attackState = AttackState.None;

    Vector3 _moveDir;
    Vector3 _dir;
    Vector3 _inputDir;
    float _attackDir;
    #endregion

    #region Init Setting
    ////////////////////////////////InitSetting////////////////////////////////
    private void InitPlayerSetting()
    {
        if (Com == null)
            Com = GetComponentInChildren<PlayerComponent>();
        ChangeHpAct += PlayerUI.Instance.ChangeHP;
        DeadAct += Death;
        
        CurHp = MaxHP;
        effectData.renderers = new Renderer[1];
        effectData.renderers[0] = Com.Myrenderer;
        effectData.mainTexture = Com.Myrenderer.material.mainTexture;
        attackMask = (int)BSLayerMasks.Monster;
        ObjectPoolManager.Instance.SetPool(Com.MyEffects, 10, 10);

        rBody.mass = 50.0f;
        rBody.constraints |= RigidbodyConstraints.FreezeRotationY;


        #region PlayerInputsCallback Setting
        ////////////////////////////////PlayerInputsCallbackSetting////////////////////////////////
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

        playerInputs.Player.MoveAnim.performed += SetAnimMove;
        playerInputs.Player.MoveAnim.canceled += SetAnimStop;
        playerInputs.Enable();
        #endregion

        //����â�� ��Ŀ���� ������ �� ����� �޼��� ���
        Application.focusChanged += OnFocusChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Application.focusChanged -= OnFocusChanged;
    }

    /// <summary> ����ȭ�� ���� Ŭ������ �� �̵��� ���� �޼��� </summary>
    void OnFocusChanged(bool hasFocus)
    {
        if (!hasFocus)
        {
            WSADInput = 0b_0000;
            Com.MyAnim.SetBool(AnimParam.isMoving, false);
        }
    }

    void Death()
    {
        Com.MyAnim.SetTrigger(AnimParam.Death);
        isOutOfControl = true;
        Com.SetRigWeight(0.0f);
        playerInputs.Disable();
    }
    #endregion

    #region Unity Event
    ////////////////////////////////UnityEvent////////////////////////////////
    protected override void Awake()
    {
        base.Awake();
        InitPlayerSetting();
    }

    private void Update()
    {
        //���� ���¸� return
        if (IsDead)
            return;
        //�Է��� ���� ���¸�
        if (WSADInput == 0b_0000)
        {
            _moveDir = Vector3.zero;
            _inputDir = Vector3.Lerp(_inputDir, Vector3.zero, Time.deltaTime * 10.0f);
        }
        else
        {
            _moveDir.Normalize();
            //�ٶ󺸴� ��������� �ִϸ��̼� ����(�Է¹��� ���⿡�� �ٶ󺸴� ������ �ݴ�������� ȸ��)
            _dir = Quaternion.AngleAxis(-Com.MyTransform.rotation.eulerAngles.y, Vector3.up) * _moveDir;
            _inputDir = Vector3.Lerp(_inputDir, _dir, Time.deltaTime * 10.0f);
        }
        Com.MyAnim.SetFloat(AnimParam.x, _inputDir.x);
        Com.MyAnim.SetFloat(AnimParam.y, _inputDir.z);
    }

    protected override void FixedUpdate()
    {
        worldMoveDir = _moveDir;
        base.FixedUpdate();
    }
    #endregion

    #region Private Method


    #endregion

    #region Public Method
    ////////////////////////////////PublicMethod////////////////////////////////
    public void OnAttackPoint()
    {
        _attackDir = (attackState == AttackState.ComboCheck) ? 180.0f : 0.0f;

        //���� ����Ʈ ����
        GameObject go = ObjectPoolManager.Instance.GetEffect(Com.GetMyEffect(), attack: Attack).This.gameObject;
        go.transform.position = Com.MyEffectSpawn.position;
        go.transform.rotation = Quaternion.Euler(0.0f, Com.MyEffectSpawn.rotation.eulerAngles.y, _attackDir);
    }

    public void ChangeAttackState(AttackState state)
    {
        attackState = state;
    }
    #endregion
}
