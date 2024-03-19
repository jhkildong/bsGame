using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CampFire : Building
{

    [SerializeField] private LayerMask findPlayerMask;
    [SerializeField]private float duration;
    [SerializeField]private short healAmount;
    [SerializeField]private float healTick;
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
    void Start()
    {
        gameObject.GetComponentInChildren<SphereCollider>().radius = healRadius; //�� ���� ����
        StartCoroutine(lifeSpan());// ���ӽð� ����
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if ((1<< other.gameObject.layer & findPlayerMask) != 0)
        {
            playerIsInRange = true;
            StartCoroutine(healTickTimeCheck(other.GetComponent<IHealing>()));
        }
    }
    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & findPlayerMask) != 0)
        {
            playerIsInRange = false;
        }

    }


    void activeHeal()
    {
        // ����� �������̽��� �����Ѵ�. ��ں� �����ȿ� �� �Ҽ� �ִ� ������Ʈ�� ������, �� �ϴ� �������. (������ getComponent -> �������̽��� �������ִ��� �Ǻ� -> ������ ��)
        //��������Ʈ �̺�Ʈ�� �־����� �ʱ� �����̴�.
        //healEvent.Invoke(healAmount);
    }

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
                Debug.Log("��!");
                healable.ReceiveHeal(healAmount);
                //activeHeal();
                inTime = 0;
            }
            yield return null;
        }
        yield return null;
    }


}
