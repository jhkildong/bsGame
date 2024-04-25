using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AttackBuilding_Meele : AttackBuildingBase
{
    [SerializeField] private MeeleAttackBuildingData MeeleBuildingData;
    public MeeleAttackBuildingData MData
    {
        get { return MeeleBuildingData; }
        set { MeeleBuildingData = value; }
    }

    public GameObject myAtkCollider; //���� ���� Collider

    [SerializeField] protected float _finalDmg; // ����������
    [SerializeField] protected float _finalRadius; // ���� ���ݹ���


    [SerializeField] private float _atkPower;
    [SerializeField] private float _hitDelay; // �ǹ������� Ÿ�� ����
    [SerializeField] private float _atkRadius; // ������ ����
    //[SerializeField] private LayerMask _attackableLayer;


    protected override void Start()
    {
        base.Start();
        attackableLayer = MData.attackableLayer;
        _atkPower = MData.atkPower;
        _hitDelay = MData.hitDelay;
        _atkRadius = MData.atkRadius;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
    }
    protected void MeeleAttack()
    {
        //SetActiveEffects();
        //MeeleAtk();
    }

    protected override void ConstructComplete() 
    { 
        base.ConstructComplete();
        myAtkCollider.SetActive(true);
    }


    public float SetDmg()
    {
        return _finalDmg;
    }
    public float SetAtkRadius()
    {
        return _atkRadius;
    }
    public float SetHitDelay()
    {
        return _hitDelay;
    }
    public LayerMask SetAttackableMask()
    {
        return attackableLayer;
    }


    /* 0412 �ӽ� �ּ�ó��. 
    protected override void AttackToTarget()
    {
        if (!atkDelaying)
        {
            atkDelaying = true;
            StartCoroutine(AtkDelay(_atkDelay));
        }
    }
    */





    /*
    void SetActiveEffects(Collider target)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].activeSelf == false)
            {
                effectList[i].transform.position = target.transform.position + new Vector3(0, 0.2f, 0);
                effectList[i].SetActive(true);
                break;
            }
            else
            {
                if (i == effectList.Count - 1)
                {
                    InstEffects();
                }
                continue;
            }
        }
    }
    */
    /*
    public void MeeleAtk()
    {
        Collider[] colliders = Physics.OverlapBox(myAtkCollider.transform.position, myAtkCollider.size, transform.rotation, attackableLayer);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                IDamage target = collider.GetComponent<IDamage>();
                if(target != null)
                {
                    target.TakeDamage(_attackPower);
                    Debug.Log("���� �ǹ� ����!");
                    SetActiveMeeleAtkEffect(collider);
                }
                
            }
        }
        
    }
    */
    /*
    protected override void OnTriggerEnter(Collider other)
    {
        AtkEvent?.Invoke();
    }

    protected void MeeleAttack()
    {
        SetActiveMeeleAtkEffect();
        MeeleAtk();
    }

    void SetActiveMeeleAtkEffect()
    {
        for (int i = 0; i < effectList.Count; i++)
        {

            if (effectList[i].activeSelf == false)
            {
                effectList[i].transform.position = target.transform.position + new Vector3(0, 0.2f, 0);
                effectList[i].SetActive(true);
                break;
            }

            else
            {
                if (i == effectList.Count - 1)
                {
                    InstEffects();
                }
                continue;
            }
        }
    }

    protected override void Update()
    {
        base.Update();

    }
    void OnDrawGizmos()
    {
        if (iscompletedBuilding)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(10, 10, 10));
        }
    }

    void MeeleAtk()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(10,10,10),Quaternion.identity,attackableLayer);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                IDamage target = collider.GetComponent<IDamage>();
                target.TakeDamage(_attackPower);
                Debug.Log("���� �ǹ� ����!");
            }
        }
    }
    */
}
