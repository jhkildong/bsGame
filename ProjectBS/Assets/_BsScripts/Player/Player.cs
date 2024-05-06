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
    public event UnityAction StartAttackAct;
    public event UnityAction EndAttackAct;
    public event UnityAction<float, float> ChangeCoolTimeAct;
    public event UnityAction<float, float> ChangeBuffTimeAct;
    public event UnityAction<int> ChangeStackAct;
    
    public Transform RotatingBody => rotatingBody;
    public bool IsBuilding
    {
        get => _isBuilding;
        set
        {
            _isBuilding = value;
            Com.MyAnim.SetBool(AnimParam.isBuilding, value);
            SetInputState(!value);
            if (value)
                rBody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            else
                rBody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
        }
    }
    public float ConstSpeed
    {
        get => _constSpeed * (1 + ConstSpeedBuff);
        set => _constSpeed = value;
    }
    public float RepairSpeed
    {
        get => _repairSpeed * (1 + RepairSpeedBuff); 
        set => _repairSpeed = value;
    }
    public float ConstSpeedBuff {get => _constSpeedBuff; set => _constSpeedBuff = value;}
    public float RepairSpeedBuff { get => _repairSpeedBuff; set => _repairSpeedBuff = value;}

    private bool _isBuilding;
    [SerializeField] private float _constSpeed;
    [SerializeField] private float _repairSpeed;
    private float _constSpeedBuff;
    private float _repairSpeedBuff;

    public void SetMoveStop(bool state)
    {
        isStop = state;
    }

    public void SetEffectAttack()
    {
        Com.Attack = Attack;
    }
    public void UpdateStatus(float attack, float coolTime, float atksp = 1.0f, float castingTime = 5.0f)
    {
        _attack = attack;
        Aksp = atksp;
        Com.MyAnim.SetFloat(AnimParam.AttackSpeed, Aksp);
        skillCoolTime = coolTime;
        if(remainCoolTime > coolTime)
            remainCoolTime = coolTime;
        maxCastingTime = castingTime;
    }
    public void SetGameManagerBuff()
    {
        getBuff.atkBuffDict["SavedAttack"] = GameManager.Instance.SaveData.Attack * 0.1f;
        getBuff.asBuffDict["SavedAksp"] = GameManager.Instance.SaveData.AkSp * 0.1f;
        getBuff.msBuffDict["SavedSpeed"] = GameManager.Instance.SaveData.MvSp * 0.1f;
        getBuff.rangeBuffDict["SavedMagnetFieldRange"] = GameManager.Instance.SaveData.MagnetFieldRange * 0.1f;
        getBuff.hpBuffDict["SavedHp"] = GameManager.Instance.SaveData.MaxHp * 0.1f;
    }

    public void SetInputState(bool state)
    {
        if (state)
        {
            SetOutOfControl(!state);
            if (isCastingSkill) //스킬 시전 중
            {
                switch (Com.MyJob)      //TODO:분리 필요
                {
                    case Job.Warrior:
                        //RotatingBody.GetComponent<LookAtPoint>().enabled = !state;//시선변경 불가능
                        break;
                    case Job.Mage:
                        RotatingBody.GetComponent<LookAtPoint>().enabled = state;   //시선 변경 가능
                        RotatingBody.GetComponent<LookAtPoint>().SetRotSpeed(0.1f);
                        break;
                    case Job.Archer:
                        RotatingBody.GetComponent<LookAtPoint>().enabled = state;   //시선 변경 가능
                        break;
                }
            }
            else
            {
                RotatingBody.GetComponent<LookAtPoint>().enabled = state;
            }
            playerInputs.Enable();
        }
        else
        {
            SetOutOfControl(!state);                                    //이동불가
            RotatingBody.GetComponent<LookAtPoint>().enabled = state;   //시선 변경 불가
            playerInputs.Disable();                                     //입력 불가
            WSADInput = 0b_0000;                                        //이동 입력 초기화
            Com.MyAnim.SetBool(AnimParam.isMoving, false);              //이동 애니메이션 초기화
            Com.MyAnim.SetBool(AnimParam.isAttacking, false);           //공격 애니메이션 초기화
        }
    }

    SkillIcon icon;
    public void SetSkillicon(SkillIcon icon)
    {
        this.icon = icon;       //스킬아이콘 캐싱(나중에 변경?)
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
        if(isCastingSkill)  //스킬 시전중이면 리턴
            return;
        Com.MyAnim.SetBool(AnimParam.isAttacking, true);
        StartAttackAct?.Invoke();
    }
    private void EndAttack(InputAction.CallbackContext context)
    {
        if (isCastingSkill)
            return;
        Com.MyAnim.SetBool(AnimParam.isAttacking, false);
        EndAttackAct?.Invoke();
    }
    private void StartSkill(InputAction.CallbackContext context)
    {
        if(isCastingSkill)  //스킬 시전중이면 리턴
            return;
        if(isSkillNotReady)  //스킬 준비가 안됬으면 리턴
        {
            Debug.Log($"쿨타임: {remainCoolTime}");
            return;
        }
        if(Com.MyJob == Job.Archer && isCastingSkill)    //궁수는 스킬중이면 리턴
            return;
        OnSkillAct?.Invoke();
    }
    private void EndSkill(InputAction.CallbackContext context)
    {
        if(Com.MyJob == Job.Archer)     //궁수면 OffskillAct가 공격 시전이기 때문에 바로 실행
        {
            if (isSkillNotReady || isCastingSkill) return;    //스킬 준비가 안되있거나, 스킬 시전 도중이면(쿨타임돌고 있는 도중 눌렀을 경우)
            if (skillCasting != null)
            {
                StopCoroutine(skillCasting);
            }    
            skillCasting = null;
            OffSkillAct?.Invoke();
            return;
        }
        else
        {
            if (!isCastingSkill)    //스킬 시전중이 아니면 리턴(자동으로 끝난 경우, 쿨타임돌고 있는 도중 눌렀을 경우)
                return;
            if (skillCasting != null)
            {
                if (isOutOfControl)
                    return;
                StopCoroutine(skillCasting);
            }
            skillCasting = null;
            OffSkillAct?.Invoke();
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
    [SerializeField] private Transform rotatingBody;
    [SerializeField] private MagnetField magnetField;
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
        GameManager.Instance.GoldChangeAct += playerUI.ChangeGoldText;
        GameManager.Instance.ExpChangeAct += playerUI.ChangeExp;
        GameManager.Instance.curSecText = playerUI.curSec; //시간 텍스트 할당
        GameManager.Instance.curMinText = playerUI.curMin; //시간 텍스트 할당
        GameManager.Instance.ChangeWood(0);
        GameManager.Instance.ChangeStone(0);
        GameManager.Instance.ChangeIron(0);
        GameManager.Instance.ChangeGold(0);
        GameManager.Instance.ChangeExp(0);

        //GameManager.Instance.GoldChangeAct

        MaxHp = Com.MyStat.MaxHp;          //최대 체력 설정 직업마다 스탯을 저장해둠
        CurHp = Com.MyStat.MaxHp;           //현재 체력 설정
        moveSpeed = Com.MyStat.Sp;          //이동속도 설정
     
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
        Com.SetSkillAct(this);      //스킬 사용시 실행할 메서드 설정
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
        GameManager.Instance.GameOver();
    }
    #endregion

    #region Unity Event
    ////////////////////////////////UnityEvent////////////////////////////////
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.Player = this;

        #region BuffChangeAct Setting
        _buff.asBuffDict.ChangeBuffAct += () => { Com.MyAnim.SetFloat(AnimParam.AttackSpeed, Aksp); };
        _buff.hpBuffDict.ChangeBuffAct += () =>
        {
            float changeHp = MaxHp - tempMaxHp;
            if (CurHp + changeHp > 0)
                CurHp += changeHp;
            else
                CurHp = 1;
            tempMaxHp = MaxHp;
        };
        _buff.msBuffDict.ChangeBuffAct += () => { moveSpeedBuff = 1 + getBuff.msBuff; };
        _buff.rangeBuffDict.ChangeBuffAct += () => { magnetField.transform.localScale = Vector3.one * (1 + getBuff.rangeBuff); };
        #endregion

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
        StartCoroutine(DelayInputEnable());
        //playerInputs.Enable();
        #endregion

        //게임창의 포커스가 변했을 시 실행될 메서드 등록
        Application.focusChanged += OnFocusChanged;
    }

    private IEnumerator DelayInputEnable()
    {
        yield return new WaitForSeconds(0.05f);
        playerInputs.Enable();
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
    ////////////////////////////////PrivateMethod////////////////////////////////
    //TODO: PlayerComponent에서 파생되는 직업들을 다시 Player를 상속받게 해서 스킬의 동작을 각 직업에서 구현하기
    //처음의 구조를 잘못잡음 시간이 없어서 일단 구현한대로 진행
    private float skillCoolTime;
    private float remainCoolTime
    {
        get => _remainCoolTime;
        set
        {
            _remainCoolTime = value;
            if (remainBuffCoolTime > 0)
                return;
            ChangeCoolTimeAct?.Invoke(value, skillCoolTime);
        }
    }
    private float _remainCoolTime;
    private float maxCastingTime;
    private bool isSkillNotReady = false;   //기존에 isSkillCoolTime에서 변경 변수명과 상태를 맞추기 위해 NotReady로 명명
    public bool isCastingSkill = false;
    public bool MageLv7SkillReady
    {
        get
        {
            if (Com.MyJobBless.CurLv >= 7)
                return _mageLv7SkillReady;
            else
                return false;
        }
        set
        {
            if (Com.MyJobBless.CurLv < 7)
                _mageLv7SkillReady = false;
            else
            {
                _mageLv7SkillReady = value;     //마법사 7레벨 달성
                if (value)
                {
                    remainCoolTime = 0.0f;
                    icon.SetIcon(3);
                }
            }
        }
    }
    private bool _mageLv7SkillReady;
    public int MaxSkillStack
    {
        get => _maxSkillStack;
        set
        {
            if(value != _maxSkillStack)
            {
                _maxSkillStack = value;
                curSkillStack = value;
                remainCoolTime = 0.0f;
                isSkillNotReady = false;
            }
        }
    }
    private int _maxSkillStack = 1;
    private int curSkillStack
    {
        get => _curSkillStack;
        set
        {
            _curSkillStack = value;
            if(MaxSkillStack > 1)
                ChangeStackAct?.Invoke(_curSkillStack);
            else
                ChangeStackAct?.Invoke(-1);
        }
    }
    private int _curSkillStack;
    private Coroutine skillCasting;
    private Coroutine ArcherSkillCoolTime;

    private float MageBuffTime = 40.0f;
    private float remainBuffTime
    {
        get => _remainBuffTime;
        set
        {
            _remainBuffTime = value;
            ChangeBuffTimeAct?.Invoke(value, MageBuffTime);
        }
    }
    private float _remainBuffTime;
    private float MageBuffCoolTime = 30.0f;
    private float remainBuffCoolTime
    {
        get => _remainBuffCoolTime;
        set
        {
            _remainBuffCoolTime = value;
            ChangeCoolTimeAct?.Invoke(value, skillCoolTime);
        }
    }
    private float _remainBuffCoolTime;

    private void OnSkill()  //스킬키를 눌렀을 때 실행
    {
        Com.MyAnim.SetBool(AnimParam.isAttacking, false);

        if(MageLv7SkillReady)
        {
            MageLv7SkillReady = false;
            Com.MyJobBless.MageLv7SkillOn();
            SetOutOfControl(true);
            RotatingBody.GetComponent<LookAtPoint>().SetRotSpeed(0.05f);
            Com.MyAnim.SetTrigger(AnimParam.Lv7Skill);
            StartCoroutine(MageBuffTimer());
            return;
        }
        
        if(Com.MyJob != Job.Archer)
        {
            isSkillNotReady = true;
            isCastingSkill = true;
            Com.MyAnim.SetBool(AnimParam.isSkill, true);
        }
        skillCasting = StartCoroutine(CastingTimer()); //최대 시전시간 이후 스킬종료
    }

    private void OffSkill()
    {
        //키다운 종료 혹은 시전시간 종료시 실행
        switch (Com.MyJob)
        {
            case Job.Mage:
            case Job.Warrior:   //마법사, 전사는 스킬시전 종료
                isCastingSkill = false;
                Com.MyAnim.SetBool(AnimParam.isSkill, false);
                StartCoroutine(SkillCoolTimer());
                return;
            case Job.Archer:    //궁수는 스킬시전
                Com.MyAnim.SetTrigger(AnimParam.OnSkill);
                curSkillStack--;
                if(curSkillStack <= 0)
                    isSkillNotReady = true;
                if (ArcherSkillCoolTime == null)
                    ArcherSkillCoolTime = StartCoroutine(ArcherSkillCoolTimer());
                return;
        }
    }

    private IEnumerator SkillCoolTimer()
    {
        if (remainBuffTime > 0)
        {
            isSkillNotReady = false;
            yield break;
        }
        remainCoolTime = skillCoolTime;
        while(remainCoolTime > 0.0f)
        {
            remainCoolTime -= Time.deltaTime;
            yield return null;
        }
        isSkillNotReady = false;
        yield break;
    }

    private IEnumerator ArcherSkillCoolTimer()
    {
        remainCoolTime = skillCoolTime;
        while (true)
        {
            remainCoolTime -= Time.deltaTime;
            if(remainCoolTime <= 0.0f)
            {
                isSkillNotReady = false;
                if (curSkillStack < MaxSkillStack)
                {
                    curSkillStack++;
                    remainCoolTime = skillCoolTime;
                }
                else
                {
                    ArcherSkillCoolTime = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator MageBuffTimer()
    {
        remainBuffTime = MageBuffTime;
        icon.SetTextColor(Color.yellow);
        icon.SetSkillDuring(true);
        while (remainBuffTime > 0.0f)
        {
            remainBuffTime -= Time.deltaTime;

            yield return null;
        }
        Debug.Log("마법사 버프 종료");
        Com.MyJobBless.MageLv7SkillOff();
        Mage mage = Com as Mage;
        mage.OffLv7SkillEffect();
        StartCoroutine(MageBuffCoolTimer());
        yield break;
    }

    private IEnumerator MageBuffCoolTimer()
    {
        remainBuffCoolTime = MageBuffCoolTime;
        icon.SetTextColor(Color.white);
        icon.SetSkillDuring(false);
        icon.SetIcon(2);
        while (remainBuffCoolTime > 0.0f)
        {
            remainBuffCoolTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("마법사 버프 쿨타임 종료");
        MageLv7SkillReady = true;
        yield break;
    }


    private IEnumerator CastingTimer()
    {
        yield return new WaitForSeconds(maxCastingTime);
        OffSkillAct?.Invoke();
        skillCasting = null;
    }

    #endregion
}
