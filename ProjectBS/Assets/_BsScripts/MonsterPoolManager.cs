using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


public class MonsterPoolManager : MonoBehaviour
{
    public bool IsReady { get; private set; }
    // 최대 개수(최대 개수 생성시 반환하는 과정에서 Destroy됨)
    private int maxCount;
    // 미리 생성해둘 개수
    private int initCount;


    private MonsterData createdMonsterData;
    private int objectId;
    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<int, IObjectPool<Monster>> ojbectPoolDic = new Dictionary<int, IObjectPool<Monster>>();

    // 생성
    Monster CreatePooledMonster()
    {
        Monster createdmonster = createdMonsterData.CreateMonster();
        createdmonster.Pool = ojbectPoolDic[objectId];

        return createdmonster;
    }

    //Get함수 사용시 호출
    void OnGetMonster(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }
    //Releas함수 사용시 호출
    void OnReleaseMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    // 풀의 최대치 보다 많으면 Realse할때 이 함수가 호출
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
        // 미리 오브젝트 생성 해놓기
        for (int i = 0; i < initCount; i++)
        {
            Monster monster = CreatePooledMonster();
            monster.gameObject.transform.SetParent(poolObj.transform);
            monster.ReleaseMonster();
        }
    }
}
