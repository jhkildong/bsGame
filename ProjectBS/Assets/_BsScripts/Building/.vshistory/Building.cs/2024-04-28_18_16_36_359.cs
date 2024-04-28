using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public abstract class Building : MonoBehaviour, IDamage, IHealing
{
    [SerializeField]
    private BuildingData buildingData;
    public virtual BuildingData Data
    {
        get { return buildingData; }
        set { buildingData = value; }
    }


    [SerializeField] protected int _id;               // 건물 ID
    [SerializeField] protected string _buildingName;  // 건물 이름
    [SerializeField] protected float _maxHp;          // 건물 최대체력
    [SerializeField] protected float _curHp;          // 건물 현재체력
    [SerializeField] protected short _requireWood;      // 나무 요구 재료개수
    [SerializeField] protected short _requireStone;      // 돌 요구 재료개수
    [SerializeField] protected short _requireIron;      // 철 요구 재료개수
    [SerializeField] protected float _constTime;     // 건물 총 건설시간
    [SerializeField] protected float _repairSpeed;    // 건물 수리속도
    [SerializeField] protected GameObject _nextUpgrade;  //다음 업그레이드 건물

    protected int layerNum;

    [SerializeField] protected bool isInstalled = false;

    [SerializeField] public bool iscompletedBuilding; // 건물의 건설 여부
    float curConstTime = 0.0f; //건설한 시간

    //Yeon 추가
    [NonSerialized] public bool isConstructing = false;  // 건설중인지 여부
    private UnityAction<float> ConstructionProgress;    // 건설 진행도 증가시 호출
    public UnityAction<bool> SelectedProgress;          // 건설 선택/해제시 호출

    public event UnityAction<float> ChangeHpAct;
    private BuildingHpBar hpBar;

    [SerializeField]private GameObject AuraEffect;
    /*
    public Building(BuildingData data)
    {
       Data = data;
        cp = Data.curHp;
    } 
    */

    //private InstantiateBuilding installBuilding;

    protected virtual void Start()
    {
        SetBuildingStat();
        Debug.Log("building" + _constTime);
        /*
        Renderer[] myRenderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in myRenderer)
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);
        }
        */

        //installBuilding.BuildingInstalled += IsInstalled;
        hpBar = UIManager.Instance.CreateUI(UIID.BuildingHpBar, CanvasType.DynamicCanvas) as BuildingHpBar; // 체력바 생성
        ChangeHpAct += hpBar.ChangeHP; //체력변동이벤트를 등록

    }
    
    public void SetBuildingStat()
    {
        _id = Data.ID;
        _buildingName = Data.buildingName;
        _maxHp = Data.maxHp;
        _curHp = Data.curHp;
        _requireWood = Data.requireWood;
        _requireStone = Data.requireStone;
        _requireIron = Data.requireIron;
        _constTime = Data.constTime;
        _repairSpeed = Data.repairSpeed;
        _nextUpgrade = Data.nextUpgrade;
        layerNum = LayerMask.NameToLayer("Building");
    }
    protected virtual void Update()
    {
        
        /*
        if (Input.GetKey(KeyCode.B))
        {
            Construction(2*Time.deltaTime);
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(20);
            Debug.Log("남은 체력" + _curHp);
        }
        */

        //////ConstructionController로 옮길것!!!!!!!!!!!!
        if (Input.GetKey(KeyCode.R))
        {
            Repair(0.1f);
        }
        if(iscompletedBuilding && _nextUpgrade != null && Input.GetKeyDown(KeyCode.U)) 
        {
            //다음 업그레이드가있고, 재화가 충족될시
            Building upgradeBD = _nextUpgrade.GetComponent<Building>();
            if (GameManager.Instance.CurWood() >= upgradeBD.Data.requireWood && GameManager.Instance.CurStone() >= upgradeBD.Data.requireStone
                && GameManager.Instance.CurIron() >= upgradeBD.Data.requireIron)
            {
                //자원소모
                GameManager.Instance.ChangeWood(-upgradeBD.Data.requireWood);
                GameManager.Instance.ChangeStone(-upgradeBD.Data.requireStone);
                GameManager.Instance.ChangeIron(-upgradeBD.Data.requireIron);
                //건물 교체
                Upgrade();

            }
            else
            {
                return;
            }
        }
        
        
    }


    public void OnInstalled() // 건물이 세팅될때(클릭으로 건설위치가 정해질때) instantiateBuilding 에서 직접 호출중
    {
        isInstalled = true;
        
        //세팅될때 자원 소모
        GameManager.Instance.ChangeWood(-_requireWood);
        GameManager.Instance.ChangeStone(-_requireStone);
        GameManager.Instance.ChangeIron(-_requireIron);

    }

    public void Construction(float constSpeed) //건설중
    {
        //canBuild상태에서 상호작용키 입력시, constructing 활성화, 건설애니메이션 시작, constructing인 동안 이벤트 호출. 

        //건설중이 아닐때, 건설중 상태로 전환
        if (!isConstructing)
        {
            ProgressBar progressBar = UIManager.Instance.GetUI(UIID.ProgressBar, CanvasType.DynamicCanvas) as ProgressBar;
            progressBar.myTarget = transform;
            ConstructionProgress += progressBar.ChnageProgress;
            SelectedProgress += progressBar.Selected;
            isConstructing = true;
        }

        //이벤트 내용
        //건물은 건설상태를 구분한다 ( 완성 , 미완성 ).
        //언제? 플레이어가 상호작용할때. -> 상호작용은 어떻게? ??
        //상호작용 방식 -> 건물이 상호작용 범위를 갖는다. 범위에 플레이어가 들어오면, 상호작용 가능 상태가 된다. 상호작용은 플레이어에서 ray를 쏴서 hit한 건물로 한다. 문제점 -> 상시 검사해야해서 자원 문제
        //
        //총 건설 시간에서 플레이어의 건설속도를 뺀 값을 뺀다.
        //총 건설시간이 0이되면 건설 완료, -> 레이어를 Building으로 변경한다. 머터리얼의 투명도를 조정한다.
        if (!iscompletedBuilding && isInstalled) //미완성 건물일때, 건설 세팅 상태일때
        {
            curConstTime += constSpeed;
            ConstructionProgress?.Invoke(curConstTime / _constTime);
            Debug.Log("건설 진행 시간 :"  + curConstTime);
            if (curConstTime >= _constTime)
            {
                ConstructComplete();
            }

            
        }


    }

    protected virtual void ConstructComplete() // 건설 완료시
    {
        iscompletedBuilding = true; // 건설 완료상태 true.
        Renderer[] completedBuildingRenderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in completedBuildingRenderer)
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f); 
        }

        gameObject.layer = layerNum; // 레이어를 Building으로 변경
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        Debug.Log("건설 완료");
    }

    protected void Upgrade() //건물 업그레이드
    {
        Vector3 pos = transform.position;
        GameObject upgradeBD = Instantiate(_nextUpgrade.gameObject,pos,transform.rotation);
        Building bd = upgradeBD.GetComponent<Building>();
        bd.isInstalled = true;
        bd.iscompletedBuilding = true;
        bd.gameObject.layer = layerNum; 
        bd.gameObject.SetActive(false);
        bd.gameObject.SetActive(true);
        Destroy();
    
    }


    public void ReceiveHeal(float heal)
    {
        Debug.Log("건물 힐 됨 현재 체력 : " + _curHp);
        _curHp += heal;
        _curHp = Mathf.Clamp(_curHp, 0, _maxHp);
    }
    public void Repair(float RepairSpeed)
    {
        if (_curHp < _maxHp)
        {
            Debug.Log("수리중" + _curHp);
            _curHp += RepairSpeed;
            if(_curHp >= _maxHp)
            {
                _curHp = _maxHp;
            }
        }

    }

    public void TakeDamage(float dmg) // IDamage 인터페이스 구현
    {
        if(iscompletedBuilding && isInstalled &&_curHp>0)
        {
            _curHp -= dmg;
            if (_curHp <= 0)
            {
                Destroy();
            }
        }
    }

    protected virtual void Destroy()
    {

        Destroy(gameObject);

    }

}
