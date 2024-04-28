using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CampFire : Building
{

    [SerializeField] private LayerMask findPlayerMask;
    [SerializeField] private float duration;
    [SerializeField] private short healAmount;
    [SerializeField] private float healTick;
    [SerializeField] private float healRadius;

    bool playerIsInRange;

    //public UnityEvent<short> healEvent;

    // Start is called before the first frame update
    void Awake()
    {
        duration = 20;
        healAmount = 5;
        healTick = 2;
        healRadius = 3f;
    }
   protected override void Start()
    {
        base.Start();
        gameObject.GetComponentInChildren<SphereCollider>().radius = healRadius; //힐 범위 설정
        
    }

    protected override void ConstructComplete() //건설 완료시
    {
        base.ConstructComplete();
        StartCoroutine(lifeSpan());// 지속시간 설정
    }


    void OnTriggerEnter(Collider other)
    {
        if ((1<< other.gameObject.layer & findPlayerMask) != 0 && iscompletedBuilding)
        {
            playerIsInRange = true;
            StartCoroutine(healTickTimeCheck(other.GetComponent<IHealing>()));
        }
    }
    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & findPlayerMask) != 0 && iscompletedBuilding)
        {
            playerIsInRange = false;
        }

    }


    void activeHeal()
    {
        // 기능을 인터페이스로 구현한다. 모닥불 범위안에 힐 할수 있는 오브젝트가 있으면, 힐 하는 방식으로. (범위안 getComponent -> 인터페이스를 가지고있는지 판별 -> 있으면 힐)
        //델리게이트 이벤트로 넣어주지 않기 위함이다.
        //healEvent.Invoke(healAmount);
    }

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

    IEnumerator healTickTimeCheck(IHealing healable)
    {
        if (healable == null) yield break;
        float inTime = 0;
        while (playerIsInRange)
        {
            if (inTime < healTick)
            {
                inTime += Time.deltaTime;
            }
            else if (inTime >= healTick)
            {
                Debug.Log("힐!");
                healable.ReceiveHealEffect(healAmount);
                //activeHeal();
                inTime = 0;
            }
            yield return null;
        }
        yield return null;
    }


}
