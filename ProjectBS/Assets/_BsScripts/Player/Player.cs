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
        if(isCoolTime)  //��Ÿ���̸� ����
        {
            Debug.Log($"��Ÿ��: {remainCoolTime}");
            return;
        }
        OnSkillAct?.Invoke();
    }
    private void EndSkill(InputAction.CallbackContext context)
    {
        if (!_isCastingSkill)    //��ų �������� �ƴϸ� ����
            return;
        OffSkillAct?.Invoke();
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
        GameManager.Instance.ExpChangeAct += playerUI.ChangeExpText;
        GameManager.Instance.ChangeWood(0);
        GameManager.Instance.ChangeStone(0);
        GameManager.Instance.ChangeIron(0);
        GameManager.Instance.ChangeGold(0);
        GameManager.Instance.ChangeExp(0);

        //GameManager.Instance.GoldChangeAct

        _maxHp = Com.MyStat.MaxHp;          //�ִ� ü�� ���� �������� ������ �����ص�
        CurHp = Com.MyStat.MaxHp;           //���� ü�� ����
        moveSpeed = Com.MyStat.Sp;          //�̵��ӵ� ����
        _attack = Com.MyStat.Ak;            //���ݷ� ����
        Com.MyAnim.SetFloat(AnimParam.AttackSpeed, Com.MyStat.AkSp);    //���ݼӵ� ����
        skillCoolTime = Com.MyStat.SkillCoolTime;       //��ų ��Ÿ�� ����
     
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
    private float skillCoolTime;
    private float remainCoolTime;
    private float maxCastingTime = 5.0f;
    private bool isCoolTime = false;
    
    private void OnSkill()  //��ųŰ�� ������ �� ����
    {
        if (isCoolTime)
            return;
        isCoolTime = true;
        _isCastingSkill = true;
        switch(Com.MyJob)
        {
            case Job.Mage:
            case Job.Warrior:   //������, ����� Ű�ٿ� ��ų����
                Com.MyAnim.SetBool(AnimParam.isSkill, true);
                break;
            case Job.Archer:    //�ü��� ���� ����
                (Com as Archer).ShowRange();
                break;
        }
        StartCoroutine(CastingTimer()); //�ִ� �����ð� ���� ��ų����
    }

    private void OffSkill()
    {
        if (!_isCastingSkill)   //��ų �������� �ƴϸ� ����
            return;
        _isCastingSkill = false;
        //Ű�ٿ� ���� Ȥ�� �����ð� ����� ����
        switch (Com.MyJob)
        {
            case Job.Mage:
            case Job.Warrior:   //������, ����� ��ų���� ����
                Com.MyAnim.SetBool(AnimParam.isSkill, false);
                break;
            case Job.Archer:    //�ü��� ��ų����
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
