using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AttackBuilding_Meele : AttackBuildingBase
{
    //ontriggerstay 상태라면 공격가능 상태로. 해당 범위안에(overlapbox)있는 모든 목표에게 데미지를 간격마다 준다.
    public GameObject myAtkCollider; //공격 범위 Collider

    
    protected override void Start()
    {
        base.Start();
        //AtkEvent.AddListener(MeeleAttack);
        Debug.Log("attackbuilding_Meele" + _constTime);
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



    protected override void AttackToTarget()
    {
        if (!atkDelaying)
        {
            atkDelaying = true;
            StartCoroutine(AtkDelay(_attackDelay));
        }
    }

    

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
                    Debug.Log("근접 건물 공격!");
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
                Debug.Log("근접 건물 공격!");
            }
        }
    }
    */
}
