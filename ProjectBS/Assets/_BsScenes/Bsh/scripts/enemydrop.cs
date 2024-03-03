using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    public GameObject DropItem;
    //드랍되는 아이템

    public int hp = 10;
    //몬스터의 체력

    //public float highValueItem = 0.5f;
    //public float middleValueItem = 7.5f;
    //public float lowValueItem;
    //드랍되는 확률

    public string boolParameter;
    //죽을때 생기는 애니매이션 이름
    public Animator animator;

    private bool hasDead = false;
    //생사 여부

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
        //죽으면 애니매이션 > 아이템 스폰 > 옵젝 사라짐.
        
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
    //반지름 2인 범위안에 아이템 드랍이 됨.

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("부딛혀서 체력이 깎임");
        hp -= 5;
    }
    //부딛히면 로그.
}
