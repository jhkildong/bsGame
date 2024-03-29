using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        public IPoolable objData;
        public int MaxCount;
        public int InitCount;

        public PoolableObjectData(IPoolable poolalbeData, int maxCount, int initCount)
        {
            objData = poolalbeData;
            MaxCount = maxCount;
            InitCount = initCount;
        }
    }

    [SerializeField]
    private List<PoolableObjectData> poolableObjectDataList = new List<PoolableObjectData>();

    private Dictionary<int, Stack<GameObject>> poolDict = new Dictionary<int, Stack<GameObject>>();


    private void Start()
    {
        Init();
    }

    #region private Method
    //저장되어있는 poolalbeData를 기반으로 pool 미리 생성
    //MonsterSpawner에서 Data를 보내줄예정이라 삭제 가능
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
                Stack<GameObject> pool = CreatePool(poolable, data.InitCount);
                poolDict.Add(poolable.ID, pool);
            }
        }
    }
    
    //게임오브젝트를 저장하는 스택 생성
    private Stack<GameObject> CreatePool(IPoolable poolable, int init)
    {
        Stack<GameObject> pool = new Stack<GameObject>(init);
        GameObject poolObj = new GameObject($"ID:{poolable.ID} pool");
        for (int i = 0; i < init; i++)
        {
            GameObject clone = poolable.CreateClone();
            clone.name += $"{i}"; //임시
            clone.transform.SetParent(poolObj.transform);
            clone.SetActive(false);
            pool.Push(clone);
        }
        return pool;
    }
    #endregion

    #region public Method
    /// <summary>풀에서 오브젝트 가져옴. 풀에 오브젝트가 없는 경우 새로 생성</summary>
    public GameObject GetObj(IPoolable poolable)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("키가 존재하지 않음");
            return null;
        }

        GameObject go;

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
        go.SetActive(true);

        return go;
    }

    public GameObject GetAttackEffect(Effect effect, float attack = 1, float speed = 1, float size = 1)
    {
        effect.Attack = attack;
        effect.Speed = speed;
        effect.Size = size;
        return GetObj(effect);
    }

    /// <summary>풀에 오브젝트 반환 최대치를 넘었을 시 순차적으로 파괴해주는 코드 작성 필요</summary>
    public void ReleaseObj(IPoolable poolable, GameObject obj)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("키가 존재하지 않음");
        }

        obj.SetActive(false);
        pool.Push(obj);
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
        Stack<GameObject> pool = CreatePool(poolable, init);
        poolDict.Add(poolable.ID, pool);
    }
    #endregion
}