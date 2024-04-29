using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sampleAction : MonoBehaviour
{
    public Transform target;
    Vector3 dir;
    public float Hp;
    public GameObject firePrefab;
    public Animator animator;

    void Start()
    {
        Hp = 100;
        target = GameObject.Find("Player").transform;
        //밑에 애니매이션에 앞으로 걸어가는거도 포함되어 있으니 주의(뒤가 가는 속도)
        animator.SetFloat("groundLocomotion", 0.02f);
        StartCoroutine(CheckState());
    }
    void Update()
    {
        transform.LookAt(target);
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
        if(target != null)
        {
            Vector3 randomPosition = target.position + Random.insideUnitSphere * 3f;
            randomPosition.y = 0f;
            Instantiate(firePrefab, randomPosition, Quaternion.identity);
            //위에 instantiate 대신 떨어지는 모션만 추가해서 데미지 넣음.
        }
    }
    IEnumerator MoveHeight(float height)
    {
        float elapsedTime = 0f;
        float duration = 3f;
        animator.SetTrigger("goAir");
        Vector3 originPosition = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, height, transform.position.z), t);
            yield return null;
        }
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
                firstPhase = false;
                StartCoroutine(MoveHeight(10f));
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(2f);
                    attackInSky();
                }
                yield return new WaitForSeconds(2f);
                StartCoroutine(MoveHeight(originY));
            }
            if (Hp <= 30 && secondPhase)
            {
                secondPhase = false;
                Debug.Log("체력 30일때 모션");
                StartCoroutine(MoveHeight(10f));
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(2f);
                    attackInSky();
                }
                yield return new WaitForSeconds(2f);
                StartCoroutine(MoveHeight(originY));
            }
            if (Hp <= 0)
            {
                //animator.SetTrigger("collapse");
                //Destroy(gameObject);
            }
            yield return null;
        }
    }

}