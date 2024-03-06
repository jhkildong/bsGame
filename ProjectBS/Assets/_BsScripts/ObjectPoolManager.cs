using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    private class ObjectInfo
    {
        public MonsterData monsterData;
        // 최대 개수(최대 개수 생성시 반환하는 과정에서 Destroy됨)
        public int maxCount;
        // 미리 생성해둘 개수
        public int initCount;
    }

    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos;

    

    private int objectId;
    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<int, IObjectPool<Monster>> ojbectPoolDic = new Dictionary<int, IObjectPool<Monster>>(); 

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        IsReady = false;

        foreach(ObjectInfo info in objectInfos)
        {
            IObjectPool<Monster> pool = new ObjectPool<Monster>(CreatePooledMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize:info.maxCount);
            ojbectPoolDic.Add(info.monsterData.ID, pool);
            objectId = info.monsterData.ID;
            
            
            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < info.initCount; i++)
            {
                Monster monster = CreatePooledMonster();
                monster.ReleaseMonster();
            }
        }

        IsReady = true;
    }

    // 생성
    Monster CreatePooledMonster()
    {
        Monster monster = objectInfos[0].monsterData.CreateMonster();
        monster.Pool = ojbectPoolDic[objectId];

        return monster;
    }

    // Called when an item is taken from the pool using Get
    void OnGetMonster(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }

    // Called when an item is returned to the pool using Release
    void OnReleaseMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyMonster(Monster monster)
    {
        Destroy(monster.gameObject);
    }

    public GameObject GetMonster(int id)
    {
        return ojbectPoolDic[id].Get().gameObject;
    }
}
