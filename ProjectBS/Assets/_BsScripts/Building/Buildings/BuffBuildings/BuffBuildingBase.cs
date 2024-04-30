using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffBuildingBase : Building
{
    public LayerMask targetLayer;
    public List<GameObject> targets;

    //bool isWorking; // targets list가 비어있지않으면 true

    /*
    protected override void ConstructComplete() //건설 완료시
    {
        base.ConstructComplete();
        StartCoroutine(lifeSpan());// 지속시간 설정
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

    protected virtual void StartBuff(Collider other) //자식에서 구현
    {

    }

    protected virtual void RemoveBuff(Collider other)//자식에서 구현
    {

    }

    public override void Destroy()
    {
        base.Destroy();
    }



    void activeHeal()
    {
        // 기능을 인터페이스로 구현한다. 모닥불 범위안에 힐 할수 있는 오브젝트가 있으면, 힐 하는 방식으로. (범위안 getComponent -> 인터페이스를 가지고있는지 판별 -> 있으면 힐)
        //델리게이트 이벤트로 넣어주지 않기 위함이다.
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
        Debug.Log("Campfire 지속시간 끝");
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
                Debug.Log("힐!");
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
