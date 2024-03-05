using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
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

    void Start()
    {
        //Chance_of_item = new float[DropItem.Length];
        //lowValueItem = 100.0f - (highValueItem + middleValueItem);
        
            
    }

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
