using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Yeon;


public class Player : Combat, IDamage<Player>
{
    #region Public Field, Method
    ////////////////////////////////Public Field, Method////////////////////////////////
    public event UnityAction OnSkillAct;
    public event UnityAction OffSkillAct;
    
    public Transform RotatingBody => rotatingBody;
    public bool IsBuilding
    {
        get => _isBuilding;
        set
        {
            _isBuilding = value;
            Com.MyAnim.SetBool(AnimParam.isBuilding, value);
            RotatingBody.GetComponent<LookAtPoint>().enabled = !value;
        }
    }
    public bool IsCastingSkill => _isCastingSkill;
    public float ConstSpeed
    {
        get => _constSpeed;
        set => _constSpeed = value;
    }
    public float RepairSpeed
    {
        get => _repairSpeed;
        set => _repairSpeed = value;
    }

    private bool _isBuilding;
    private bool _isCastingSkill;
    [SerializeField] private float _constSpeed;
    [SerializeField] private float _repairSpeed;

    public void SetEffectAttack()
    {
        Com.Attack = Attack;
    }
    #endregion
    
    #region PlayerInput
    ////////////////////////////////PlayerInput////////////////////////////////
    // 플레이어의 키 조작을 버튼 형식으로 받음
    // 상하, 좌우를 동시에 입력시 나중에 입력된 동작으로 덮어씌워짐
    // 버튼을 떼어낼 때 비트 연산을 통해 이전 입력이 있었는지 확인한 후 입력값 설정
    ///////////////////////////////////////////////////////////////////////////

    private PlayerInputs playerInputs;
    private void StartAttack(InputAction.CallbackContext context)
    {
        Com.MyAnim.SetBool(AnimParam.isAttacking, true);
    }
    private void EndAttack(InputAction.CallbackContext context)
    {
        Com.MyAnim.SetBool(AnimParam.isAttacking, false);
    }
    private void StartSkill(InputAction.CallbackContext context)
    {
        if(isCoolTime)  //쿨타임이면 리턴
        {
            Debug.Log($"쿨타임: {remainCoolTime}");
            return;
        }
        OnSkillAct?.Invoke();
    }
    private void EndSkill(InputAction.CallbackContext context)
    {
        if (!_isCastingSkill)    //스킬 시전중이 아니면 리턴
            return;
        OffSkillAct?.Invoke();
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
    [SerializeField] private Transform rotatingBody;
    private PlayerUI playerUI;

    Vector3 _moveDir;
    Vector3 _dir;
    Vector3 _inputDir;

    #endregion

    #region Init Setting
    ////////////////////////////////InitSetting////////////////////////////////
    public void InitPlayerSetting(PlayerComponent com)
    {
        Com = com;
        playerUI = UIManager.Instance.CreateUI(UIID.PlayerUI, CanvasType.DynamicCanvas) as PlayerUI;    //플레이어 UI생성 이부분은 고민해봐야함
        ChangeHpAct += playerUI.ChangeHP;   //체력이 변할때 UI에 반영
        DeadAct += Death;                   //죽었을 때 실행할 메서드 등록

        GameManager.Instance.WoodChangeAct += playerUI.ChangeWoodText;
        GameManager.Instance.StoneChangeAct += playerUI.ChangeStoneText;
        GameManager.Instance.IronChangeAct += playerUI.ChangeIronText;
        GameManager.Instance.ExpChangeAct += playerUI.ChangeExpText;
        GameManager.Instance.ChangeWood(0);
        GameManager.Instance.ChangeStone(0);
        GameManager.Instance.ChangeIron(0);
        GameManager.Instance.ChangeGold(0);
        GameManager.Instance.ChangeExp(0);

        //GameManager.Instance.GoldChangeAct

        _maxHp = Com.MyStat.MaxHp;          //최대 체력 설정 직업마다 스탯을 저장해둠
        CurHp = Com.MyStat.MaxHp;           //현재 체력 설정
        moveSpeed = Com.MyStat.Sp;          //이동속도 설정
        _attack = Com.MyStat.Ak;            //공격력 설정
        Com.MyAnim.SetFloat(AnimParam.AttackSpeed, Com.MyStat.AkSp);    //공격속도 설정
        skillCoolTime = Com.MyStat.SkillCoolTime;       //스킬 쿨타임 설정
     
        effectData.SetRenderer(Com.Myrenderers);        //피격 이펙트 설정

        ObjectPoolManager.Instance.SetPool(Com.MyEffect, 10, 10);   //공격 이펙트 오브젝트 풀 설정

        attackMask = (int)BSLayerMasks.Monster;     //공격할 레이어 설정

        //rigidbody 설정
        rBody.mass = 50.0f;
        rBody.constraints |= RigidbodyConstraints.FreezeRotationY;

        //유니티 이벤트 등록
        Com.MyAnimEvent.AttackAct += SetEffectAttack;   //애니메이션에서 공격 하는 시점에서 실행
        Com.MyAnimEvent.AttackAct += Com.OnAttackPoint;

        OnSkillAct += OnSkill;      //스킬키를 눌렀을 때 실행
        OffSkillAct += OffSkill;    //스킬키를 떼었을 때 실행
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
        GameManager.Instance.Player = this;

        #region PlayerInputsCallback Setting
        ////////////////////////////////PlayerInputsCallbackSetting////////////////////////////////
        playerInputs = new PlayerInputs();
        playerInputs.Player.Attack.performed += StartAttack;
        playerInputs.Player.Attack.canceled += EndAttack;

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

        playerInputs.Player.Skill.performed += StartSkill;
        playerInputs.Player.Skill.canceled += EndSkill;
        playerInputs.Enable();
        #endregion

        //게임창의 포커스가 변했을 시 실행될 메서드 등록
        Application.focusChanged += OnFocusChanged;
    }

    private void Update()
    {
        //죽은 상태거나 건물 짓는중이면 return
        if (IsDead || _isBuilding)
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
            _dir = Quaternion.AngleAxis(-rotatingBody.rotation.eulerAngles.y, Vector3.up) * _moveDir;
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
    private float skillCoolTime;
    private float remainCoolTime;
    private float maxCastingTime = 5.0f;
    private bool isCoolTime = false;
    
    private void OnSkill()  //스킬키를 눌렀을 때 실행
    {
        if (isCoolTime)
            return;
        isCoolTime = true;
        _isCastingSkill = true;
        switch(Com.MyJob)
        {
            case Job.Mage:
            case Job.Warrior:   //마법사, 전사는 키다운 스킬시전
                Com.MyAnim.SetBool(AnimParam.isSkill, true);
                break;
            case Job.Archer:    //궁수는 범위 지정
                (Com as Archer).ShowRange();
                break;
        }
        StartCoroutine(CastingTimer()); //최대 시전시간 이후 스킬종료
    }

    private void OffSkill()
    {
        if (!_isCastingSkill)   //스킬 시전중이 아니면 리턴
            return;
        _isCastingSkill = false;
        //키다운 종료 혹은 시전시간 종료시 실행
        switch (Com.MyJob)
        {
            case Job.Mage:
            case Job.Warrior:   //마법사, 전사는 스킬시전 종료
                Com.MyAnim.SetBool(AnimParam.isSkill, false);
                break;
            case Job.Archer:    //궁수는 스킬시전
                Com.MyAnim.SetTrigger(AnimParam.OnSkill);
                return;
        }
        StartCoroutine(SkillCoolTimer());
    }

    private IEnumerator SkillCoolTimer()
    {
        remainCoolTime = skillCoolTime;
        while(remainCoolTime > 0.0f)
        {
            remainCoolTime -= Time.deltaTime;
            yield return null;
        }
        isCoolTime = false;
        yield break;
    }

    private IEnumerator CastingTimer()
    {
        yield return new WaitForSeconds(maxCastingTime);
        OffSkillAct?.Invoke();
    }

    #endregion
}
