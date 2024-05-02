using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Yeon;

[DisallowMultipleComponent]
public class ObjectPoolManager : MonoBehaviour
{

    /// <summary> 싱글턴 매니저 </summary>
    private static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectPoolManager>();
                if (_instance == null) return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


    ///<summary>풀에 등록할 오브젝트들의 데이터 인스펙터창에 보이기 위해 ScirptableObject로 필드를 설정함</summary>
    [Serializable]
    public class PoolableObjectData
    {
        public IPoolable objData; //임시
        public MonoBehaviour Data; //임시
        public int ID => _id;
        public int MaxCount;
        public int InitCount;

        [SerializeField] private int _id; //임시
        public PoolableObjectData(IPoolable poolalbeData, int maxCount, int initCount)
        {
            objData = poolalbeData;
            MaxCount = maxCount;
            InitCount = initCount;
            _id = poolalbeData.ID;
            Data = poolalbeData.This;
        }
    }

    [SerializeField]
    private List<PoolableObjectData> poolableObjectDataList = new List<PoolableObjectData>();

    private Dictionary<int, Stack<IPoolable>> poolDict = new Dictionary<int, Stack<IPoolable>>();


    private void OnEnable()
    {
        Init();
    }

    #region private Method
    //저장되어있는 poolalbeData를 기반으로 pool 미리 생성
    //MonsterSpawner에서 Data를 보내줄예정이라 코드 삭제 가능
    private void Init()
    {
        int len = poolableObjectDataList.Count;
        if (len == 0) return;

        foreach(PoolableObjectData data in poolableObjectDataList)
        {
            if(data.objData is IPoolable poolable)
            {
                if (poolDict.ContainsKey(poolable.ID)) //중복된 키값
                {
                    Debug.Log("이미 존재하는 오브젝트 입니다");
                    return;
                }
                Stack<IPoolable> pool = CreatePool(poolable, data.InitCount);
                poolDict.Add(poolable.ID, pool);
            }
        }
    }
    
    //IPoolable를 저장하는 스택 생성
    private Stack<IPoolable> CreatePool(IPoolable poolable, int init)
    {
        Stack<IPoolable> pool = new Stack<IPoolable>(init);
        GameObject poolObj = new GameObject($"ID:{poolable.ID} pool");
        for (int i = 0; i < init; i++)
        {
            IPoolable clone = poolable.CreateClone();
            clone.This.gameObject.name += $"{i}"; //임시
            clone.This.transform.SetParent(poolObj.transform);
            clone.This.gameObject.SetActive(false);
            pool.Push(clone);
        }
        return pool;
    }
    #endregion

    #region public Method
    /// <summary>풀에서 오브젝트 가져옴. 풀에 오브젝트가 없는 경우 새로 생성</summary>
    public IPoolable GetObj(IPoolable poolable)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("키가 존재하지 않음");
            return null;
        }

        IPoolable go;

        // 1. 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 2. 재고가 없는 경우 원본으로부터 복제
        else
        {
            go = poolable.CreateClone();
        }
        go.This.gameObject.SetActive(true);
        return go;
    }

    public PlayerAttackType GetEffect(PlayerAttackType poolable, float attack = 1, float size = 1)
    {
        IPoolable go = GetObj(poolable);
        PlayerAttackType effect = go.This as PlayerAttackType;
        effect.Attack = attack;
        effect.Size = size;

        return effect;
    }

    /// <summary>풀에 오브젝트 반환 최대치를 넘었을 시 순차적으로 파괴해주는 코드 작성 필요</summary>
    public void ReleaseObj(IPoolable poolable)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("키가 존재하지 않음");
            return;
        }

        poolable.This.gameObject.SetActive(false);
        pool.Push(poolable);
    }

    /// <summary>풀에 등록</summary>
    public void SetPool(IPoolable poolable, int max, int init)
    {
        if (poolDict.ContainsKey(poolable.ID)) //중복된 키값
        {
            Debug.Log("이미 존재하는 오브젝트 입니다");
            return;
        }
        poolableObjectDataList.Add(new PoolableObjectData(poolable, max, init));
        Stack<IPoolable> pool = CreatePool(poolable, init);
        poolDict.Add(poolable.ID, pool);
    }

    public void SetPool(IPoolable[] poolables, int max, int init)
    {
        foreach(IPoolable poolable in poolables)
        {
            SetPool(poolable, max, init);
        }
    }
    #endregion
}