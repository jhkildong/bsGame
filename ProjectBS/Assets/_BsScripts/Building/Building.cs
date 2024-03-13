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

    private int _id;               // �ǹ� ID
    private string _buildingName;  // �ǹ� �̸�
    private short _maxHp;          // �ǹ� �ִ�ü��
    private short _curHp;          // �ǹ� ����ü��
    private short _requireWood;      // ���� �䱸 ��ᰳ��
    private short _requireStone;      // �� �䱸 ��ᰳ��
    private short _requireIron;      // ö �䱸 ��ᰳ��
    private float _constTime;     // �ǹ� �� �Ǽ��ð�
    private float _repairSpeed;    // �ǹ� �����ӵ�
    
    
    private int layerNum;

    [SerializeField] private bool isInstalled = false;

    private bool completedBuilding; // �ǹ��� �Ǽ� ����
    float curConstTime = 0.0f; //�Ǽ��� �ð�



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
            Debug.Log("���� ü��" + _curHp);
        }
        if (Input.GetKey(KeyCode.R))
        {
            Repair(0.1f);
        }



    }


    public void OnInstalled() // �ǹ��� ���õɶ�(Ŭ������ �Ǽ���ġ�� ��������)
    {
        isInstalled = true;
    }
    
    void Construction(float constSpeed) //�Ǽ���
    {
        //canBuild���¿��� ��ȣ�ۿ�Ű �Է½�, constructing Ȱ��ȭ, �Ǽ��ִϸ��̼� ����, constructing�� ���� �̺�Ʈ ȣ��. 



        //�̺�Ʈ ����
        //�ǹ��� �Ǽ����¸� �����Ѵ� ( �ϼ� , �̿ϼ� ).
        //����? �÷��̾ ��ȣ�ۿ��Ҷ�. -> ��ȣ�ۿ��� ���? ??
        //��ȣ�ۿ� ��� -> �ǹ��� ��ȣ�ۿ� ������ ���´�. ������ �÷��̾ ������, ��ȣ�ۿ� ���� ���°� �ȴ�. ��ȣ�ۿ��� �÷��̾�� ray�� ���� hit�� �ǹ��� �Ѵ�. ������ -> ��� �˻��ؾ��ؼ� �ڿ� ����
        //
        //�� �Ǽ� �ð����� �÷��̾��� �Ǽ��ӵ��� �� ���� ����.
        //�� �Ǽ��ð��� 0�̵Ǹ� �Ǽ� �Ϸ�, -> ���̾ Building���� �����Ѵ�. ���͸����� ������ �����Ѵ�.
        if (!completedBuilding && isInstalled) //�̿ϼ� �ǹ��϶�, �Ǽ� ���� �����϶�
        {
            curConstTime += constSpeed;
            Debug.Log("�Ǽ� ���� �ð� :"  + curConstTime);
            if (curConstTime >= _constTime)
            {
                ConstructComplete();
            }
        }


    }

    void ConstructComplete() // �Ǽ� �Ϸ��
    {
        completedBuilding = true; // �Ǽ� �Ϸ���� true.
        Renderer[] completedBuildingRenderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in completedBuildingRenderer)
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f); //���� 0.5
        }

        gameObject.layer = layerNum; // ���̾ Building���� ����
        Debug.Log("�Ǽ� �Ϸ�");
    }

    void Repair(float RepairSpeed)
    {
        if (_curHp < _maxHp)
        {
            Debug.Log("������" + _curHp);
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
