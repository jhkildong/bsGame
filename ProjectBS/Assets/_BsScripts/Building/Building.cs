using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public abstract class Building : MonoBehaviour
{
    [SerializeField]
    private BuildingData buildingData;
    public BuildingData Data
    {
        get { return buildingData; }
        set { buildingData = value; }
    }

    private int _id;               // 건물 ID
    private string _buildingName;  // 건물 이름
    private short _maxHp;          // 건물 최대체력
    private short _curHp;          // 건물 현재체력
    private short _requireWood;      // 나무 요구 재료개수
    private short _requireStone;      // 돌 요구 재료개수
    private short _requireIron;      // 철 요구 재료개수
    private float _constTime;     // 건물 총 건설시간
    private float _repairSpeed;    // 건물 수리속도
    
    
    private int layerNum;

    [SerializeField] private bool isInstalled = false;

    private bool completedBuilding; // 건물의 건설 여부
    float curConstTime = 0.0f; //건설한 시간



    /*
    public Building(BuildingData data)
    {
       Data = data;
        cp = Data.curHp;
    } 
    */

    //private InstantiateBuilding installBuilding;

    void Start()
    {
        _maxHp = Data.maxHp;
        _curHp = Data.curHp;
        _constTime = Data.constTime;
        _repairSpeed = Data.repairSpeed;
        layerNum = LayerMask.NameToLayer("Building");

        /*
        Renderer[] myRenderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in myRenderer)
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);
        }
        */
        
        //installBuilding.BuildingInstalled += IsInstalled;

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            Construction(2*Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GetDamage(20);
            Debug.Log("남은 체력" + _curHp);
        }
        if (Input.GetKey(KeyCode.R))
        {
            Repair(0.1f);
        }



    }


    public void OnInstalled() // 건물이 세팅될때(클릭으로 건설위치가 정해질때)
    {
        isInstalled = true;
    }
    
    void Construction(float constSpeed) //건설중
    {
        //canBuild상태에서 상호작용키 입력시, constructing 활성화, 건설애니메이션 시작, constructing인 동안 이벤트 호출. 



        //이벤트 내용
        //건물은 건설상태를 구분한다 ( 완성 , 미완성 ).
        //언제? 플레이어가 상호작용할때. -> 상호작용은 어떻게? ??
        //상호작용 방식 -> 건물이 상호작용 범위를 갖는다. 범위에 플레이어가 들어오면, 상호작용 가능 상태가 된다. 상호작용은 플레이어에서 ray를 쏴서 hit한 건물로 한다. 문제점 -> 상시 검사해야해서 자원 문제
        //
        //총 건설 시간에서 플레이어의 건설속도를 뺀 값을 뺀다.
        //총 건설시간이 0이되면 건설 완료, -> 레이어를 Building으로 변경한다. 머터리얼의 투명도를 조정한다.
        if (!completedBuilding && isInstalled) //미완성 건물일때, 건설 세팅 상태일때
        {
            curConstTime += constSpeed;
            Debug.Log("건설 진행 시간 :"  + curConstTime);
            if (curConstTime >= _constTime)
            {
                ConstructComplete();
            }
        }


    }

    void ConstructComplete() // 건설 완료시
    {
        completedBuilding = true; // 건설 완료상태 true.
        Renderer[] completedBuildingRenderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in completedBuildingRenderer)
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f); //투명도 0.5
        }

        gameObject.layer = layerNum; // 레이어를 Building으로 변경
        Debug.Log("건설 완료");
    }

    void Repair(float RepairSpeed)
    {
        if (_curHp < _maxHp)
        {
            Debug.Log("수리중" + _curHp);
            _curHp += (short)RepairSpeed;
            if(_curHp >= _maxHp)
            {
                _curHp = _maxHp;
            }
        }

    }

    void GetDamage(short dmg)
    {
        if(completedBuilding && isInstalled)
        {
            _curHp -= dmg;
            if (_curHp <= 0)
            {
                Destroy();
            }
        }

    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
