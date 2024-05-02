using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Yeon;

[DisallowMultipleComponent]
public class ObjectPoolManager : MonoBehaviour
{

    /// <summary> �̱��� �Ŵ��� </summary>
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


    ///<summary>Ǯ�� ����� ������Ʈ���� ������ �ν�����â�� ���̱� ���� ScirptableObject�� �ʵ带 ������</summary>
    [Serializable]
    public class PoolableObjectData
    {
        public IPoolable objData; //�ӽ�
        public MonoBehaviour Data; //�ӽ�
        public int ID => _id;
        public int MaxCount;
        public int InitCount;

        [SerializeField] private int _id; //�ӽ�
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
    //����Ǿ��ִ� poolalbeData�� ������� pool �̸� ����
    //MonsterSpawner���� Data�� �����ٿ����̶� �ڵ� ���� ����
    private void Init()
    {
        int len = poolableObjectDataList.Count;
        if (len == 0) return;

        foreach(PoolableObjectData data in poolableObjectDataList)
        {
            if(data.objData is IPoolable poolable)
            {
                if (poolDict.ContainsKey(poolable.ID)) //�ߺ��� Ű��
                {
                    Debug.Log("�̹� �����ϴ� ������Ʈ �Դϴ�");
                    return;
                }
                Stack<IPoolable> pool = CreatePool(poolable, data.InitCount);
                poolDict.Add(poolable.ID, pool);
            }
        }
    }
    
    //IPoolable�� �����ϴ� ���� ����
    private Stack<IPoolable> CreatePool(IPoolable poolable, int init)
    {
        Stack<IPoolable> pool = new Stack<IPoolable>(init);
        GameObject poolObj = new GameObject($"ID:{poolable.ID} pool");
        for (int i = 0; i < init; i++)
        {
            IPoolable clone = poolable.CreateClone();
            clone.This.gameObject.name += $"{i}"; //�ӽ�
            clone.This.transform.SetParent(poolObj.transform);
            clone.This.gameObject.SetActive(false);
            pool.Push(clone);
        }
        return pool;
    }
    #endregion

    #region public Method
    /// <summary>Ǯ���� ������Ʈ ������. Ǯ�� ������Ʈ�� ���� ��� ���� ����</summary>
    public IPoolable GetObj(IPoolable poolable)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("Ű�� �������� ����");
            return null;
        }

        IPoolable go;

        // 1. Ǯ�� ��� �ִ� ��� : ��������
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 2. ��� ���� ��� �������κ��� ����
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

    /// <summary>Ǯ�� ������Ʈ ��ȯ �ִ�ġ�� �Ѿ��� �� ���������� �ı����ִ� �ڵ� �ۼ� �ʿ�</summary>
    public void ReleaseObj(IPoolable poolable)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("Ű�� �������� ����");
            return;
        }

        poolable.This.gameObject.SetActive(false);
        pool.Push(poolable);
    }

    /// <summary>Ǯ�� ���</summary>
    public void SetPool(IPoolable poolable, int max, int init)
    {
        if (poolDict.ContainsKey(poolable.ID)) //�ߺ��� Ű��
        {
            Debug.Log("�̹� �����ϴ� ������Ʈ �Դϴ�");
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