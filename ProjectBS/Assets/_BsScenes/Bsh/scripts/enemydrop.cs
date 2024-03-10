using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    /// <summary>
    /// ���, �߱�, �ϱ� ������ Ȯ�� �� ������ ��� ���� �� ����.
    /// Ȯ���� %�� ������ ��.
    /// �ڵ�ȿ� ������ ������ ������ ĳ���Ͱ� ����.
    /// </summary>
    public float PChance;
    public ItemData[] PremiumDropItem;
    public float MChance;
    public ItemData[] MiddleDropItem;
    public float BChance;
    public ItemData[] BasicDropItem;
    //����Ǵ� ������
    public int hp = 10;
    private bool hasDead = false;
    //���� ����
    void Update()
    {
        if(hp <= 0 && !hasDead)
        {
            hasDead = true;
            SpawnItem();
            DestroyObject();
        }
        //������ �ִϸ��̼� > ������ ���� > ���� �����.      
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
            Debug.Log("��޾����� ����");
        }
        else if (i <= PChance + MChance)
        {
            int num = Random.Range(0, MiddleDropItem.Length);
            Instantiate(MiddleDropItem[num].ItemPrefab, rnd, Quaternion.identity);
            Debug.Log("�߱޾����� ����");
        }
        else
        {
            int num = Random.Range(0, BasicDropItem.Length);
            Instantiate(BasicDropItem[num].ItemPrefab, rnd, Quaternion.identity);
            Debug.Log("�ϱ޾����� ����");
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        hp -= 5;
        Debug.Log("�ε����� ü���� ����");
    }
}
