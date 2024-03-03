using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GridBrushBase;

public class ReturnToPool : MonoBehaviour
{
    public IObjectPool<GameObject> Pool;

    public void ReleaseObject()
    {
        Pool.Release(gameObject);
    }
}

public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    private class ObjectInfo
    {
        public ScriptableObject objData;
        // �ִ� ����
        public int maxCount;
        // ���� ����
        public int initCount;
    }

    public bool IsReady { get; private set; }

    // �̹� Ǯ�� �ִ� �׸��� �������Ϸ��� �ϸ� ���� �߻�.
    public bool collectionChecks = true;

    IObjectPool<GameObject> m_Pool;

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

     // ������ ������Ʈ�� key�������� ���� ����
    private int objectID;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<int, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<int, IObjectPool<GameObject>>();

    // ������ƮǮ���� ������Ʈ�� ���� �����Ҷ� ����� ��ųʸ�
    private Dictionary<int, GameObject> goDic = new Dictionary<int, GameObject>();



    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            if (objectInfos[idx].objData is MonsterData md)
            {
                IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                                                                            collectionChecks, objectInfos[idx].initCount, objectInfos[idx].maxCount);
                goDic.Add(md.ID, md.MonsterPrefab);
                ojbectPoolDic.Add(md.ID, pool);

                // �̸� ������Ʈ ���� �س���
                for (int i = 0; i < objectInfos[idx].initCount; i++)
                {
                    objectID = md.ID;
                    CreatePooledItem();
                }
            }
        }

        IsReady = true;
    }

    // ����
    GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectID]);
        // This is used to return ParticleSystems to the pool when they have stopped.
        var returnToPool = poolGo.AddComponent<ReturnToPool>();
        returnToPool.Pool = ojbectPoolDic[objectID];
        returnToPool.ReleaseObject();

        return poolGo;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject poolObj)
    {
        poolObj.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject poolObj)
    {
        poolObj.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject poolObj)
    {
        Destroy(poolObj.gameObject);
    }

    public GameObject GetGo(int goID)
    {
        objectID = goID;

        if (goDic.ContainsKey(goID) == false)
        {
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", goID);
            return null;
        }

        return ojbectPoolDic[goID].Get();
    }
}
