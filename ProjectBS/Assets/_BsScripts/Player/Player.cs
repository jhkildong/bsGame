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
    public event UnityAction<float, float> ChangeCoolTimeAct;
    public event UnityAction<int> ChangeStackAct;
    
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
        maxCastingTime = castingTime;
    }
    #endregion

    #region PlayerInput
    ////////////////////////////////PlayerInput////////////////////////////////
    // �÷��̾��� Ű ������ ��ư �������� ����
    // ����, �¿츦 ���ÿ� �Է½� ���߿� �Էµ� �������� �������
    // ��ư�� ��� �� ��Ʈ ������ ���� ���� �Է��� �־����� Ȯ���� �� �Է°� ����
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
        if(isSkillNotReady)  //��ų �غ� �ȉ����� ����
        {
            Debug.Log($"��Ÿ��: {remainCoolTime}");
            return;
        }
        if(Com.MyJob == Job.Archer && isCastingSkill)    //�ü��� ��ų���̸� ����
            return;
        OnSkillAct?.Invoke();
    }
    private void EndSkill(InputAction.CallbackContext context)
    {
        if(Com.MyJob == Job.Archer)     //�ü��� OffskillAct�� ���� �����̱� ������ �ٷ� ����
        {
            if (isSkillNotReady || isCastingSkill) return;    //��ų �غ� �ȵ��ְų�, ��ų ���� �����̸�(��Ÿ�ӵ��� �ִ� ���� ������ ���)
            if (skillCasting != null)
                StopCoroutine(skillCasting);
            skillCasting = null;
            OffSkillAct?.Invoke();
            return;
        }
        else
        {
            if (!isCastingSkill)    //��ų �������� �ƴϸ� ����(�ڵ����� ���� ���, ��Ÿ�ӵ��� �ִ� ���� ������ ���)
                return;
            if (skillCasting != null)
                StopCoroutine(skillCasting);
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
        playerUI = UIManager.Instance.CreateUI(UIID.PlayerUI, CanvasType.DynamicCanvas) as PlayerUI;    //�÷��̾� UI���� �̺κ��� ����غ�����
        ChangeHpAct += playerUI.ChangeHP;   //ü���� ���Ҷ� UI�� �ݿ�
        DeadAct += Death;                   //�׾��� �� ������ �޼��� ���

        GameManager.Instance.WoodChangeAct += playerUI.ChangeWoodText;
        GameManager.Instance.StoneChangeAct += playerUI.ChangeStoneText;
        GameManager.Instance.IronChangeAct += playerUI.ChangeIronText;
        GameManager.Instance.GoldChangeAct += playerUI.ChangeGoldText;
        GameManager.Instance.ExpChangeAct += playerUI.ChangeExp;
        GameManager.Instance.ChangeWood(0);
        GameManager.Instance.ChangeStone(0);
        GameManager.Instance.ChangeIron(0);
        GameManager.Instance.ChangeGold(0);
        GameManager.Instance.ChangeExp(0);

        //GameManager.Instance.GoldChangeAct

        MaxHp = Com.MyStat.MaxHp;          //�ִ� ü�� ���� �������� ������ �����ص�
        CurHp = Com.MyStat.MaxHp;           //���� ü�� ����
        moveSpeed = Com.MyStat.Sp;          //�̵��ӵ� ����
     
        effectData.SetRenderer(Com.Myrenderers);        //�ǰ� ����Ʈ ����

        ObjectPoolManager.Instance.SetPool(Com.MyEffect, 10, 10);   //���� ����Ʈ ������Ʈ Ǯ ����

        attackMask = (int)BSLayerMasks.Monster;     //������ ���̾� ����

        //rigidbody ����
        rBody.mass = 50.0f;
        rBody.constraints |= RigidbodyConstraints.FreezeRotationY;

        //����Ƽ �̺�Ʈ ���
        Com.MyAnimEvent.AttackAct += SetEffectAttack;   //�ִϸ��̼ǿ��� ���� �ϴ� �������� ����
        Com.MyAnimEvent.AttackAct += Com.OnAttackPoint;

        OnSkillAct += OnSkill;      //��ųŰ�� ������ �� ����
        OffSkillAct += OffSkill;    //��ųŰ�� ������ �� ����
        Com.SetSkillAct(this);      //��ų ���� ������ �޼��� ����
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
        _buff.msBuffDict.ChangeBuffAct += () => { moveSpeed *= (1 + getBuff.msBuff); };
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
        playerInputs.Enable();
        #endregion

        //����â�� ��Ŀ���� ������ �� ����� �޼��� ���
        Application.focusChanged += OnFocusChanged;
    }

    private void Update()
    {
        //���� ���°ų� �ǹ� �������̸� return
        if (IsDead || _isBuilding)
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
    //TODO: PlayerComponent���� �Ļ��Ǵ� �������� �ٽ� Player�� ��ӹް� �ؼ� ��ų�� ������ �� �������� �����ϱ�
    //ó���� ������ �߸����� �ð��� ��� �ϴ� �����Ѵ�� ����
    private float skillCoolTime;
    private float remainCoolTime
    {
        get => _remainCoolTime;
        set
        {
            _remainCoolTime = value;
            ChangeCoolTimeAct?.Invoke(value, skillCoolTime);
        }
    }
    private float _remainCoolTime;
    private float maxCastingTime;
    private bool isSkillNotReady = false;   //������ isSkillCoolTime���� ���� ������� ���¸� ���߱� ���� NotReady�� ���
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
                _mageLv7SkillReady = value;
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
    private float remainBuffTime;
    private float MageBuffCoolTime = 30.0f;
    private float reaminBuffCoolTime;
    
    private void OnSkill()  //��ųŰ�� ������ �� ����
    {
        if(MageLv7SkillReady)
        {
            MageLv7SkillReady = false;
            Com.MyJobBless.MageLv7SkillOn();
            SetOutOfControl(true);
            RotatingBody.GetComponent<LookAtPoint>().SetRotSpeed(0.1f);
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
        skillCasting = StartCoroutine(CastingTimer()); //�ִ� �����ð� ���� ��ų����
    }

    private void OffSkill()
    {
        //Ű�ٿ� ���� Ȥ�� �����ð� ����� ����
        switch (Com.MyJob)
        {
            case Job.Mage:
            case Job.Warrior:   //������, ����� ��ų���� ����
                isCastingSkill = false;
                Com.MyAnim.SetBool(AnimParam.isSkill, false);
                StartCoroutine(SkillCoolTimer());
                return;
            case Job.Archer:    //�ü��� ��ų����
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
        while (remainBuffTime > 0.0f)
        {
            remainBuffTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("������ ���� ����");
        Com.MyJobBless.MageLv7SkillOff();
        Mage mage = Com as Mage;
        mage.OffLv7SkillEffect();
        StartCoroutine(MageBuffCoolTimer());
        yield break;
    }

    private IEnumerator MageBuffCoolTimer()
    {
        reaminBuffCoolTime = MageBuffCoolTime;
        while (reaminBuffCoolTime > 0.0f)
        {
            reaminBuffCoolTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("������ ���� ��Ÿ�� ����");
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
