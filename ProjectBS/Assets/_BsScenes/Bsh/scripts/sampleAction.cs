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
        StartCoroutine(CheckState());
        target = gameObject.transform.Find("Player");
        animator = gameObject.GetComponent<Animator>();
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
        Debug.Log("������");
        //Vector3 go = transform.position - target.position;
        if(target != null)
        {
            
            //�Ӹ� ���� ����.
            //�÷��̾� ��ġ�� �����Ѵ�
            Instantiate(firePrefab, target.position, Quaternion.identity);
            //���� instantiate ��� �������� ��Ǹ� �߰��ؼ� ������ ����.
        }
    }
    IEnumerator MoveHeight(float height)
    {
        float elapsedTime = 0f;
        float duration = 3f;
        animator.SetTrigger("goAir");
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, height, transform.position.z), t);
            yield return null;
        }
        Debug.Log("�ö�.");
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
                attackInSky();
                yield return new WaitForSeconds(5f);
                StartCoroutine(MoveHeight(originY));
            }
            if (Hp <= 30 && secondPhase)
            {
                secondPhase = false;
                Debug.Log("ü�� 30�϶� ���");
                StartCoroutine(MoveHeight(10f));
                attackInSky();
                yield return new WaitForSeconds(5f);
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