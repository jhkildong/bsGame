using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    /// <summary>
    /// 상급, 중급, 하급 아이템 확률 및 아이템 드랍 정할 수 있음.
    /// 확률은 %로 적으면 됨.
    /// 코드안에 죽으면 아이템 스폰과 캐릭터가 죽음.
    /// </summary>
    public float PChance;
    public ItemData[] PremiumDropItem;
    public float MChance;
    public ItemData[] MiddleDropItem;
    public float BChance;
    public ItemData[] BasicDropItem;
    //드랍되는 아이템
    public int hp = 10;
    private bool hasDead = false;
    //생사 여부
    void Update()
    {
        if(hp <= 0 && !hasDead)
        {
            hasDead = true;
            SpawnItem();
            DestroyObject();
        }
        //죽으면 애니매이션 > 아이템 스폰 > 옵젝 사라짐.      
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void SpawnItem()
    {
        Vector3 rnd = transform.position + Random.insideUnitSphere * 2.0f;
        rnd.y = 1.0f;
        float i = Random.Range(0.0f, 100.0f);
        if (i <= PChance)
        {
            int num = Random.Range(0, PremiumDropItem.Length);
            Instantiate(PremiumDropItem[num].ItemPrefab, rnd, Quaternion.identity);
            Debug.Log("상급아이템 생성");
        }
        else if (i <= PChance + MChance)
        {
            int num = Random.Range(0, MiddleDropItem.Length);
            Instantiate(MiddleDropItem[num].ItemPrefab, rnd, Quaternion.identity);
            Debug.Log("중급아이템 생성");
        }
        else
        {
            int num = Random.Range(0, BasicDropItem.Length);
            Instantiate(BasicDropItem[num].ItemPrefab, rnd, Quaternion.identity);
            Debug.Log("하급아이템 생성");
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        hp -= 5;
        Debug.Log("부딛혀서 체력이 깎임");
    }
}
