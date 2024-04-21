using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffBuildingBase : Building
{

    public enum Type
    {
        Heal, Buff
    }

    public LayerMask targetLayer;
    public List<GameObject> targets;
    //bool isWorking; // targets list�� ������������� true

    /*
    protected override void ConstructComplete() //�Ǽ� �Ϸ��
    {
        base.ConstructComplete();
        StartCoroutine(lifeSpan());// ���ӽð� ����
    }
    */
    protected override void Update()
    {
        base.Update();
        /*
        if (targets.Count==0) 
        {
            isWorking = false;
        }
        else
        {
            isWorking = true;
        }
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & targetLayer) != 0 && iscompletedBuilding)
        {
            if(other != null)
            {
                targets.Add(other.gameObject);
                StartBuff(other);
                //StartCoroutine(healTickTimeCheck(other.GetComponent<IHealing>(),other.gameObject));
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & targetLayer) != 0 && iscompletedBuilding)
        {
            if (targets.Contains(other.gameObject))
            {
                targets.Remove(other.gameObject);
                RemoveBuff(other);
            }
        }
    }

    protected virtual void StartBuff(Collider other) //�ڽĿ��� ����
    {

    }

    protected virtual void RemoveBuff(Collider other)//�ڽĿ��� ����
    {

    }

    protected override void Destroy()
    {
        base.Destroy();
    }



    void activeHeal()
    {
        // ����� �������̽��� �����Ѵ�. ��ں� �����ȿ� �� �Ҽ� �ִ� ������Ʈ�� ������, �� �ϴ� �������. (������ getComponent -> �������̽��� �������ִ��� �Ǻ� -> ������ ��)
        //��������Ʈ �̺�Ʈ�� �־����� �ʱ� �����̴�.
        //healEvent.Invoke(healAmount);
    }

    /*
    IEnumerator lifeSpan()
    {
        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Campfire ���ӽð� ��");
        Destroy(gameObject);
    }
    */
    /*
    IEnumerator healTickTimeCheck(IHealing healable,GameObject obj)
    {
        if (obj == null) yield break;
        float inTime = 0;
        while (obj!=null&&targets.Contains(obj))
        {
            if (inTime < healTick)
            {
                inTime += Time.deltaTime;
            }
            else if (inTime >= healTick)
            {
                Debug.Log("��!");
                if (targets.Contains(obj))
                {
                    healable.ReceiveHeal(healAmount);
                }
                else
                {
                    
                }
                //activeHeal();
                inTime = 0;
            }
            yield return null;
        }
        if(targets.Contains(obj)) targets.Remove(obj);
        yield return null;
    }
    */
}
