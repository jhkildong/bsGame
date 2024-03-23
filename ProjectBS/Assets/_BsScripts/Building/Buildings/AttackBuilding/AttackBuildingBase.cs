using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackBuildingBase : Building
{
    // Start is called before the first frame update



    [SerializeField]protected GameObject target;
    public List<GameObject> detectedObj = new List<GameObject>();
    public bool atkDelaying;

    public LayerMask attackableLayer;

    public GameObject atkEffect;
    public List<GameObject> effectList; //
    public Transform effectPool; //������Ʈ Ǯ�� �� �ʱ� ��ġ. 

    public UnityEvent AtkEvent;


    protected short _attackPower;// ���ݰ����� �ǹ��� ���ݷ�
    public float _attackDelay;  // �ǹ��� ���� ������
    protected float _attackRadius; // �ǹ��� ���� ������ (��ǥ ������ ����)
    protected float _attackProjectileSize; //�ǹ��� ����ü ������ (����ü�� ����)

    /*
    public short Damage
    {
        get { return _attackPower; }
        set { _attackPower = value; }
    }
    */
    protected override void Start()
    {
        base.Start();
        _attackPower = Data.attackPower;
        _attackDelay = Data.attackDelay;
        _attackRadius = Data.attackRadius;
        _attackProjectileSize = Data.attackProjectileSize;


        Debug.Log("attackbuilding" + _constTime);
    }

    public void SetAtkStats()
    {
        //���ȿ� ������ ������ ȣ��� �Լ�.

    }
    void OnTriggerEnter(Collider other)
    {
        if (iscompletedBuilding)
        {
            SetAttackTarget(other);
        }
    }
    protected void SetAttackTarget(Collider other) //���� ���� �����ȿ� ���� ���� Ÿ������
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                detectedObj.Add(other.gameObject);
                target = detectedObj[0];
                //Debug.Log(target);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0 && iscompletedBuilding)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                if (detectedObj.Contains(other.gameObject))
                {
                    detectedObj.Remove(other.gameObject);
                    if(detectedObj.Count > 0)
                    {
                        target = detectedObj[0]; //���ο� Ÿ�� ã��
                    }
                    else
                    {
                        target = null;
                    }
                    //Debug.Log(other.gameObject);
                }
            }
        }
    }


    protected override void Update()
    {
        base.Update();

        if(target!=null)
        {
            if(!atkDelaying)
            {
                atkDelaying = true;
                StartCoroutine(AtkDelay(_attackDelay));
            }
        }
    }

    IEnumerator AtkDelay(float delay)
    {
        Debug.Log("����!");
        //���⸦ �̺�Ʈ�� ȣ���� �Լ��� �߰��ؾߵ�.
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    protected override void ConstructComplete()
    {
        base.ConstructComplete();

        InstEffects();

    }

    protected void InstEffects() // ����Ʈ ������Ʈ Ǯ�� (����)
    {
        /*
        for (int i = 0; i < 5; i++)
        {
            GameObject eft = Instantiate(atkEffect, effectPool);

            effectList.Add(eft);
            eft.SetActive(false);
        }
        */

        GameObject eft = Instantiate(atkEffect, effectPool);

        effectList.Add(eft);
        eft.SetActive(false);


    }


    /*
    protected void OnMeeleAttack(Collider other)
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                obj.TakeDamage(_attackPower);
                //Debug.Log(obj);
            }
        }
    }
    */





    /*
    public short SetDmg()
    {
        return  _attackPower;
    }
    */

    public short SetDmg()
    {
        return _attackPower;
    }
    public float SetAtkRadius()
    {
        return _attackRadius;
    }
    public float SetAtkProjectileSize()
    {
        return _attackProjectileSize;
    }


}
