using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    public GameObject DropItem;
    //����Ǵ� ������

    public int hp = 10;
    //������ ü��

    //public float highValueItem = 0.5f;
    //public float middleValueItem = 7.5f;
    //public float lowValueItem;
    //����Ǵ� Ȯ��

    public string boolParameter;
    //������ ����� �ִϸ��̼� �̸�
    public Animator animator;

    private bool hasDead = false;
    //���� ����

    void Start()
    {
        //lowValueItem = 100.0f - (highValueItem + middleValueItem);
    }

    void Update()
    {
        if(hp <= 0 && !hasDead)
        {
            hasDead = true;
            StartCoroutine(DeathAnimation());
            SpawnItem();
            DestroyObject();
        }
        //������ �ִϸ��̼� > ������ ���� > ���� �����.
        
    }
    IEnumerator DeathAnimation()
    {
        animator.SetBool(boolParameter, true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void SpawnItem()
    {
        Vector3 rnd = transform.position + Random.insideUnitSphere * 2.0f;
        rnd.y = 1.0f;
        Instantiate(DropItem, rnd, Quaternion.identity);
    }
    //������ 2�� �����ȿ� ������ ����� ��.

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�ε����� ü���� ����");
        hp -= 5;
    }
    //�ε����� �α�.
}
