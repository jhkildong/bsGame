using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuilding : Building
{
    // Start is called before the first frame update
    public enum AttackType
    {
        Meele, //근거리 타입
        Range // 원거리 타입 (투사체 발사 타입)
    }

    public GameObject target;
    public List<GameObject> detectedObj = new List<GameObject>();
    public bool atkDelaying;

    AttackType atkType;
    public LayerMask attackableLayer;


    void OnTriggerEnter(Collider other)
    {
        SetRangeAttackTarget(other);
    }
    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                if (detectedObj.Contains(other.gameObject))
                {
                    detectedObj.Remove(other.gameObject);
                    Debug.Log(other.gameObject);
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if(atkType == AttackType.Range && target!=null)
        {
            if(!atkDelaying)
            {
                AtkDelay(_attackDelay);
            }
        }
    }



    protected void OnMeeleAttack(Collider other)
    {
        if (atkType == AttackType.Meele && (1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                obj.TakeDamage(_attack);
                //Debug.Log(obj);
            }
        }
    }

    


    protected void SetRangeAttackTarget(Collider other) //가장 먼저 범위안에 들어온 적을 타겟으로
    {
        if (atkType == AttackType.Range && (1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                detectedObj.Add(other.gameObject);
                target = detectedObj[0];
                Debug.Log(target);
            }
        }
    }

    protected virtual void RangeAttack() // 자식에서 구현
    {
       
    }



    IEnumerator AtkDelay(float delay)
    {
        atkDelaying = true;
        RangeAttack();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }

}
