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
    // 플레이어의 키 조작을 버튼 형식으로 받음
    // 상하, 좌우를 동시에 입력시 나중에 입력된 동작으로 덮어씌워짐
    // 버튼을 떼어낼 때 비트 연산을 통해 이전 입력이 있었는지 확인한 후 입력값 설정
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
        WSADInput |= W; //W를 눌렀다고 표시
    }
    private void PressS(InputAction.CallbackContext context)
    {
        _moveDir.z = -1.0f;
        WSADInput |= S; //S를 눌렀다고 표시
    }
    private void PressA(InputAction.CallbackContext context)
    {
        _moveDir.x = -1.0f;
        WSADInput |= A; //A를 눌렀다고 표시
    }
    private void PressD(InputAction.CallbackContext context)
    {
        _moveDir.x = 1.0f;
        WSADInput |= D; //D를 눌렀다고 표시
    }
    private void ReleaseW(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~W; //W를 떼었다고 표시
        _moveDir.z = (WSADInput & S) != 0 ? -1.0f : 0f; //S가 눌려있으면 -1, 아니면 0
    }
    private void ReleaseS(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~S; //S를 떼었다고 표시
        _moveDir.z = (WSADInput & W) != 0 ? 1.0f : 0f; //W가 눌려있으면 1, 아니면 0
    }
    private void ReleaseA(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~A;  //A를 떼었다고 표시
        _moveDir.x = (WSADInput & D) != 0 ? 1.0f : 0f; //D가 눌려있으면 1, 아니면 0
    }
    private void ReleaseD(InputAction.CallbackContext context)
    {
        WSADInput &= (byte)~D;  //D를 떼었다고 표시
        _moveDir.x = (WSADInput & A) != 0 ? -1.0f : 0f; //A가 눌려있으면 -1, 아니면 0
    }
    private void SetAnimMove(InputAction.CallbackContext context)
    {
        Com.MyAnim.SetBool(AnimParam.isMoving, true); //WSAD 중 하나라도 눌려있으면 이동중
    }
    private void SetAnimStop(InputAction.CallbackContext context)
    {
        if (WSADInput != 0b_0000)  //WSAD 중 하나라도 눌려있으면 리턴
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

        //게임창의 포커스가 변했을 시 실행될 메서드 등록
        Application.focusChanged += OnFocusChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Application.focusChanged -= OnFocusChanged;
    }

    /// <summary> 게임화면 밖을 클릭했을 때 이동을 멈출 메서드 </summary>
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
        //죽은 상태면 return
        if (IsDead)
            return;
        //입력이 없는 상태면
        if (WSADInput == 0b_0000)
        {
            _moveDir = Vector3.zero;
            _inputDir = Vector3.Lerp(_inputDir, Vector3.zero, Time.deltaTime * 10.0f);
        }
        else
        {
            _moveDir.Normalize();
            //바라보는 방향기준의 애니메이션 방향(입력받은 방향에서 바라보는 방향의 반대방향으로 회전)
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

        //공격 이펙트 생성
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
