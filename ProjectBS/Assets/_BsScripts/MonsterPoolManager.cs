using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


public class MonsterPoolManager : MonoBehaviour
{
    public bool IsReady { get; private set; }
    // �ִ� ����(�ִ� ���� ������ ��ȯ�ϴ� �������� Destroy��)
    private int maxCount;
    // �̸� �����ص� ����
    private int initCount;


    private MonsterData createdMonsterData;
    private int objectId;
    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<int, IObjectPool<Monster>> ojbectPoolDic = new Dictionary<int, IObjectPool<Monster>>();

    // ����
    Monster CreatePooledMonster()
    {
        Monster createdmonster = createdMonsterData.CreateMonster();
        createdmonster.Pool = ojbectPoolDic[objectId];

        return createdmonster;
    }

    //Get�Լ� ���� ȣ��
    void OnGetMonster(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }
    //Releas�Լ� ���� ȣ��
    void OnReleaseMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    // Ǯ�� �ִ�ġ ���� ������ Realse�Ҷ� �� �Լ��� ȣ��
    void OnDestroyMonster(Monster monster)
    {
        Destroy(monster.gameObject);
    }

    public GameObject GetMonster(int id)
    {
        return ojbectPoolDic[id].Get().gameObject;
    }
    
    public void SetMonsterPool(MonsterData data, int init, int max)
    {
        createdMonsterData = data;
        initCount = init;
        maxCount = max;
        IObjectPool<Monster> pool = new ObjectPool<Monster>(CreatePooledMonster, OnGetMonster, OnReleaseMonster,
            OnDestroyMonster, maxSize: maxCount);
        ojbectPoolDic.Add(data.ID, pool);
        objectId = data.ID;

        GameObject poolObj = new($"{data.Name} pool");
        // �̸� ������Ʈ ���� �س���
        for (int i = 0; i < initCount; i++)
        {
            Monster monster = CreatePooledMonster();
            monster.gameObject.transform.SetParent(poolObj.transform);
            monster.ReleaseMonster();
        }
    }
}
