using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    //����Ǿ��ִ� poolalbeData�� ������� pool �̸� ����
    //MonsterSpawner���� Data�� �����ٿ����̶� ���� ����
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
                Stack<GameObject> pool = CreatePool(poolable, data.InitCount);
                poolDict.Add(poolable.ID, pool);
            }
        }
    }
    
    //���ӿ�����Ʈ�� �����ϴ� ���� ����
    private Stack<GameObject> CreatePool(IPoolable poolable, int init)
    {
        Stack<GameObject> pool = new Stack<GameObject>(init);
        GameObject poolObj = new GameObject($"ID:{poolable.ID} pool");
        for (int i = 0; i < init; i++)
        {
            GameObject clone = poolable.CreateClone();
            clone.name += $"{i}"; //�ӽ�
            clone.transform.SetParent(poolObj.transform);
            clone.SetActive(false);
            pool.Push(clone);
        }
        return pool;
    }
    #endregion

    #region public Method
    /// <summary>Ǯ���� ������Ʈ ������. Ǯ�� ������Ʈ�� ���� ��� ���� ����</summary>
    public GameObject GetObj(IPoolable poolable)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("Ű�� �������� ����");
            return null;
        }

        GameObject go;

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

    /// <summary>Ǯ�� ������Ʈ ��ȯ �ִ�ġ�� �Ѿ��� �� ���������� �ı����ִ� �ڵ� �ۼ� �ʿ�</summary>
    public void ReleaseObj(IPoolable poolable, GameObject obj)
    {
        int ID = poolable.ID;
        if (!poolDict.TryGetValue(ID, out var pool))
        {
            Debug.Log("Ű�� �������� ����");
        }

        obj.SetActive(false);
        pool.Push(obj);
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
        Stack<GameObject> pool = CreatePool(poolable, init);
        poolDict.Add(poolable.ID, pool);
    }
    #endregion
}