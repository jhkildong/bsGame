using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public abstract class Building : MonoBehaviour , IDamage
{
    [SerializeField]
    private BuildingData buildingData;
    public BuildingData Data
    {
        get { return buildingData; }
        set { buildingData = value; }
    }


    [SerializeField] protected int _id;               // �ǹ� ID
    [SerializeField] protected string _buildingName;  // �ǹ� �̸�
    [SerializeField] protected short _maxHp;          // �ǹ� �ִ�ü��
    [SerializeField] protected short _curHp;          // �ǹ� ����ü��
    [SerializeField] protected short _requireWood;      // ���� �䱸 ��ᰳ��
    [SerializeField] protected short _requireStone;      // �� �䱸 ��ᰳ��
    [SerializeField] protected short _requireIron;      // ö �䱸 ��ᰳ��
    [SerializeField] protected float _constTime;     // �ǹ� �� �Ǽ��ð�
    [SerializeField] protected float _repairSpeed;    // �ǹ� �����ӵ�

    protected int layerNum;

    [SerializeField] protected bool isInstalled = false;

    protected bool iscompletedBuilding; // �ǹ��� �Ǽ� ����
    float curConstTime = 0.0f; //�Ǽ��� �ð�



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

    }
    public void SetBuildingStat()
    {
        _maxHp = Data.maxHp;
        _curHp = Data.curHp;
        _constTime = Data.constTime;
        _repairSpeed = Data.repairSpeed;
        layerNum = LayerMask.NameToLayer("Building");
    }
    protected virtual void Update()
    {
        
        if (Input.GetKey(KeyCode.B))
        {
            Construction(2*Time.deltaTime);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(20);
            Debug.Log("���� ü��" + _curHp);
        }
        
        if (Input.GetKey(KeyCode.R))
        {
            Repair(0.1f);
        }
        
        
    }


    public void OnInstalled() // �ǹ��� ���õɶ�(Ŭ������ �Ǽ���ġ�� ��������) instantiateBuilding ���� ���� ȣ����
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
        if (!iscompletedBuilding && isInstalled) //�̿ϼ� �ǹ��϶�, �Ǽ� ���� �����϶�
        {
            curConstTime += constSpeed;
            Debug.Log("�Ǽ� ���� �ð� :"  + curConstTime);
            if (curConstTime >= _constTime)
            {
                ConstructComplete();
            }
        }


    }

    protected virtual void ConstructComplete() // �Ǽ� �Ϸ��
    {
        iscompletedBuilding = true; // �Ǽ� �Ϸ���� true.
        Renderer[] completedBuildingRenderer = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in completedBuildingRenderer)
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f); 
        }

        gameObject.layer = layerNum; // ���̾ Building���� ����
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        Debug.Log("�Ǽ� �Ϸ�");
    }

    public void Repair(float RepairSpeed)
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

    public void TakeDamage(short dmg) // IDamage �������̽� ����
    {
        if(iscompletedBuilding && isInstalled)
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
