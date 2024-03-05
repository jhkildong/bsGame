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
        // �ִ� ����
        public int maxCount;
        // ���� ����
        public int initCount;
    }

    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    private int objectId;
    // ������ƮǮ���� ������ ��ųʸ�
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
            IObjectPool<Monster> pool = new ObjectPool<Monster>(CreatePooledMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster,
                                                                defaultCapacity:info.initCount, maxSize:info.maxCount);
            ojbectPoolDic.Add(info.monsterData.ID, pool);
            objectId = info.monsterData.ID;
            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < info.initCount; i++)
            {
                //objectID = md.ID;
                CreatePooledMonster();
            }
        }

        IsReady = true;
    }

    // ����
    Monster CreatePooledMonster()
    {
        Monster monster = objectInfos[0].monsterData.CreateMonster();
        monster.Pool = ojbectPoolDic[objectId];
        monster.DestoryMonster();

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

    public GameObject GetGo(int id)
    {
        return ojbectPoolDic[id].Get().gameObject;
    }


}
