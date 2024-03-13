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

    public UnityEvent<short> healEvent;

    // Start is called before the first frame update
    void Awake()
    {
        duration = 10;
        healAmount = 5;
        healTick = 2;
        healRadius = 3f;
    }
    void Start()
    {
        gameObject.GetComponent<SphereCollider>().radius = healRadius;
        StartCoroutine(lifeSpan());
    }

    // Update is called once per frame
    void Update()
    {
        //범위에 들어온 player 태그를 가진 캐릭터를 주기적으로 회복시킴

    }

    void OnTriggerEnter(Collider other)
    {
        if ((1<< other.gameObject.layer & findPlayerMask) != 0)
        {
            Debug.Log("ㅎㅇ");
            playerIsInRange = true;
            StartCoroutine(healTickTimeCheck());
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("ㅂㅇ");
        playerIsInRange = false;
    }


    void activeHeal()
    {
        healEvent.Invoke(healAmount);
        Debug.Log("힐!");
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

    IEnumerator healTickTimeCheck()
    {
        float inTime = 0;
        while (playerIsInRange)
        {
            if (inTime < healTick)
            {
                inTime += Time.deltaTime;
            }
            else if (inTime >= healTick)
            {
                activeHeal();
                inTime = 0;
            }
            yield return null;
        }
        yield return null;
    }


}
