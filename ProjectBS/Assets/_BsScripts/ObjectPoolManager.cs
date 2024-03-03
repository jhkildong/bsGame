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
        // 최대 갯수
        public int maxCount;
        // 생성 갯수
        public int initCount;
    }

    public bool IsReady { get; private set; }

    // 이미 풀에 있는 항목을 릴리스하려고 하면 오류 발생.
    public bool collectionChecks = true;

    IObjectPool<GameObject> m_Pool;

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

     // 생성할 오브젝트의 key값지정을 위한 변수
    private int objectID;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<int, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<int, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할때 사용할 딕셔너리
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

                // 미리 오브젝트 생성 해놓기
                for (int i = 0; i < objectInfos[idx].initCount; i++)
                {
                    objectID = md.ID;
                    CreatePooledItem();
                }
            }
        }

        IsReady = true;
    }

    // 생성
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
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", goID);
            return null;
        }

        return ojbectPoolDic[goID].Get();
    }
}
