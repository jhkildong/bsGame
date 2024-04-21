using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    //public float AttackRange = 3;
    public Vector3 A;

    public GameObject itemprefab;
    void Start()
    {
        target = GameObject.Find("Player")?.transform;
        if (target == null)
        {
            Debug.LogError("Player를 찾을 수 없습니다.");
            return;
        }
        StartCoroutine(MoveAndAttack());
    }

    // Update is called once per frame
    void Update()
    {
        /*A = target.position - gameObject.transform.position;
        if (A.magnitude > 3)
        {
            if (target != null)
            {
                dir = target.position - transform.position;
                transform.position += dir.normalized * Time.deltaTime;
            }
        }
        else if (A.magnitude < 3)
        {
            gameObject.transform.position += Vector3.up;
        }*/
    }

    IEnumerator SplashDamage()
    {
        if (target != null)
        {
            dir = target.position - transform.position;
            transform.position += dir.normalized * Time.deltaTime;
        }
        yield return null;
    }
    IEnumerator MoveAndAttack()
    {
        while (true)
        {
            if (target != null && (target.position - transform.position).magnitude >= 3)
            {
                dir = target.position - transform.position;
                transform.position += dir.normalized * Time.deltaTime;
            }
            else if (target != null && (target.position - transform.position).magnitude < 3)
            {
                Instantiate(itemprefab, target.position, Quaternion.identity);
                Debug.Log("dd");
                yield return new WaitForSeconds(2f); // 공격 후 2초간 대기
            }
            yield return null;
        }
    }

}
