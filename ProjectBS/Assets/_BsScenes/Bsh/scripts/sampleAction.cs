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
        animator.SetFloat("groundLocomotion", 0.5f);
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
            //���� instantiate ��� �������� ��Ǹ� �߰��ؼ� ������ ����.
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
                Debug.Log("ü�� 60�϶� ���");
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
                Debug.Log("ü�� 30�϶� ���");
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