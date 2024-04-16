using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sampleAction : MonoBehaviour
{
    public Transform target;
    Vector3 dir;
    public float Hp;
    public GameObject firePrefab;

    void Start()
    {
        Hp = 100;
        StartCoroutine(CheckState());
        target = gameObject.transform.Find("Player");
    }
    void Update()
    {
        /*if(target != null)
        {
            dir = target.position - transform.position;
            transform.position += dir.normalized * Time.deltaTime;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        Hp -= 20;
        Debug.Log(Hp);
        if(other.gameObject.layer == 16)
        { 
            target = other.gameObject.transform;
        }
    }
    void attackInSky()
    {
        Debug.Log("공격함");
        //Vector3 go = transform.position - target.position;
        if(target != null)
        {
            //머리 위를 돈다.
            //플레이어 위치에 공격한다
            Instantiate(firePrefab, target.position, Quaternion.identity);
            //위에 instantiate 대신 떨어지는 모션만 추가해서 데미지 넣음.
        }
    }
    IEnumerator MoveHeight(float height)
    {
        float elapsedTime = 0f;
        float duration = 3f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, height, transform.position.z), t);
            yield return null;
        }
        Debug.Log("올라감.");
    }

    IEnumerator CheckState()
    {
        bool firstPhase = true;
        bool secondPhase = true;
        float originY = transform.position.y;

        while (true)
        {
            if (Hp <= 60 && firstPhase)
            {
                Debug.Log("체력 60일때 모션");
                // 날아 오르는 애니매이션 추가
                firstPhase = false;
                StartCoroutine(MoveHeight(10f));
                attackInSky();
                yield return new WaitForSeconds(5f);
                StartCoroutine(MoveHeight(originY));
            }
            if (Hp <= 30 && secondPhase)
            {
                secondPhase = false;
                Debug.Log("체력 30일때 모션");
                StartCoroutine(MoveHeight(10f));
                attackInSky();
                yield return new WaitForSeconds(5f);
                StartCoroutine(MoveHeight(originY));
            }
            yield return null;
        }
    }
}