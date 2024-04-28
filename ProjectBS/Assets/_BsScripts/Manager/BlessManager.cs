using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeon2;

public class BlessManager : Singleton<BlessManager>
{
    //Resources/UI ������ �ִ� BlessData�� ������ ��ųʸ�
    public Dictionary<int, BlessData> BlessDict => _blessDict;
    
    private Dictionary<int, BlessData> _blessDict;
    private WeightedRandomPicker<BlessData> blessDataWeight;
    private List<int> keyList;
    private SelectWindow blessSelectWindow;
    private Stack<UnityAction> callStack;

    BlessData[] temp;
    int[] weights;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        //UIComponent�� ��ӹ��� ��� Ŭ������ Resources/UI �������� �ε��Ͽ� ��ųʸ��� ����
        BlessData[] BlessLists = Resources.LoadAll<BlessData>(FilePath.Bless);
        _blessDict = new Dictionary<int, BlessData>();
        keyList = new List<int>();
        callStack = new Stack<UnityAction>();
        blessDataWeight = new WeightedRandomPicker<BlessData>();
        
        foreach (BlessData data in BlessLists)
        {
            _blessDict.Add(data.ID, data);
        }
        keyList.AddRange(_blessDict.Keys);
        foreach(var item in _blessDict)
        {
            blessDataWeight.Add(item.Value, 1);
        }
    }

    public Bless CreateBless(BlessID id)
    {
        if (_blessDict.ContainsKey((int)id))
        {
            Bless clone = _blessDict[(int)id].CreateClone();
            clone.transform.position += new Vector3(0, 0.7f, 0);
            return clone;
        }
        return null;
    }

    public void AppearRandomBlessList()
    {
        //�����ϴ� �����찡 ������� ����
        if(blessSelectWindow == null)
        {
            blessSelectWindow = UIManager.Instance.CreateUI(UIID.BlessSelectWindow, CanvasType.Canvas) as SelectWindow;
        }
        //�����찡 Ȱ��ȭ ������ ��� callStack�� ����
        if(blessSelectWindow.gameObject.activeSelf == true)
            callStack.Push(() => AppearRandomBlessList());
        //�ƴ� ��� Ȱ��ȭ
        else
            blessSelectWindow.gameObject.SetActive(true);

        temp = new BlessData[3];
        weights = new int[3];
        //�������� 3���� ������ ����
        for (int i = 0; i < 3; i++)
        {
            temp[i] = blessDataWeight.GetRandomPick();
            weights[i] = blessDataWeight.GetWeight(temp[i]);
            Debug.Log($"{temp[i].name}: {weights[i]}");
            blessDataWeight.Remove(temp[i]);
        }

        string[] names = new string[3];
        for(int i = 0; i < 3; i++)
        {
            names[i] = temp[i].Name;
        }
        blessSelectWindow.SelectButtons.SetButtonName(names);
        for(int i = 0; i < 3; i++)
        {
            int idx = i;
            blessSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectBless(idx));
        }
    }

    public void SelectBless(int idx)
    {
        //���õ� ������ �÷��̾�� �ο�
        CreateBless((BlessID)temp[idx].ID);
        //���õ� ������ WeightedRandomPicker�� �߰�
        for(int i = 0; i < 3; i++)
        {
            if(i == idx)
                blessDataWeight.Add(temp[i], weights[i] + 10);
            else
                blessDataWeight.Add(temp[i], weights[i]);
        }
        //���õ� ������ ������ �� callStack�� ����� �Լ� ����
        if(callStack.Count > 0)
        {
            callStack.Pop().Invoke();
        }
        else
        {
            blessSelectWindow.gameObject.SetActive(false);
        }
    }
}

public enum BlessID
{
    BOTS = 1010, RA = 1020, PF = 1030,
    LD = 1040, BOTCA = 1050, CS = 1060, VG = 1070,
    LP = 1080, RM = 1090, BOL = 1100,
}


/*
Reference By https://rito15.github.io/posts/unity-toy-weighted-random-picker/ 
*/
public class WeightedRandomPicker<T>
{
    public int SumOfWeights
    {
        get
        {
            CalculateSumIfDirty();
            return _sumOfWeights;
        }
    }

    private Dictionary<T, int> itemWeightDict;
    private Dictionary<T, float> normalizedItemWeightDict; // Ȯ���� ����ȭ�� ������ ���


    /// <summary> ����ġ ���� ������ ���� �������� ���� </summary>
    private bool isDirty;
    private int _sumOfWeights;

    /***********************************************************************
    *                               Constructors
    ***********************************************************************/
    #region .
    public WeightedRandomPicker()
    {
        itemWeightDict = new Dictionary<T, int>();
        normalizedItemWeightDict = new Dictionary<T, float>();
        isDirty = true;
        _sumOfWeights = 0;
    }
    #endregion
    /***********************************************************************
    *                               Add Methods
    ***********************************************************************/
    #region .

    /// <summary> ���ο� ������-����ġ �� �߰� </summary>
    public void Add(T item, int weight)
    {
        CheckDuplicatedItem(item);
        CheckValidWeight(weight);

        itemWeightDict.Add(item, weight);
        isDirty = true;
    }

    /// <summary> ���ο� ������-����ġ �ֵ� �߰� </summary>
    public void Add(params (T item, int weight)[] pairs)
    {
        foreach (var pair in pairs)
        {
            CheckDuplicatedItem(pair.item);
            CheckValidWeight(pair.weight);

            itemWeightDict.Add(pair.item, pair.weight);
        }
        isDirty = true;
    }

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region .

    /// <summary> ��Ͽ��� ��� ������ ���� </summary>
    public void Remove(T item)
    {
        CheckNotExistedItem(item);

        itemWeightDict.Remove(item);
        isDirty = true;
    }

    /// <summary> ��� �������� ����ġ ���� </summary>
    public void ModifyWeight(T item, int weight)
    {
        CheckNotExistedItem(item);
        CheckValidWeight(weight);

        itemWeightDict[item] = weight;
        isDirty = true;
    }
    #endregion

    /***********************************************************************
    *                               Getter Methods
    ***********************************************************************/
    #region .

    /// <summary> ���� �̱� </summary>
    public T GetRandomPick()
    {
        // ���� ���
        float chance = Random.value; // [0.0, 1.0)
        chance *= SumOfWeights;

        return GetRandomPick(chance);
    }

    /// <summary> ���� ���� ���� �����Ͽ� �̱� </summary>
    public T GetRandomPick(float randomValue)
    {
        if (randomValue < 0.0) return default;
        if (randomValue > SumOfWeights) randomValue = SumOfWeights;

        float current = 0;
        foreach (var pair in itemWeightDict)
        {
            current += pair.Value;

            if (randomValue <= current)
            {
                return pair.Key;
            }
        }

        //������� �Դٸ� ���� �߸��� ��
        return default;
    }

    /// <summary> ��� �������� ����ġ Ȯ�� </summary>
    public int GetWeight(T item)
    {
        return itemWeightDict[item];
    }

    /// <summary> ��� �������� ����ȭ�� ����ġ Ȯ�� </summary>
    public float GetNormalizedWeight(T item)
    {
        CalculateSumIfDirty();
        return normalizedItemWeightDict[item];
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region .
    /// <summary> ��� �������� ����ġ �� ����س��� </summary>
    private void CalculateSumIfDirty()
    {
        if (!isDirty) return;
        isDirty = false;

        _sumOfWeights = 0;
        foreach (var pair in itemWeightDict)
        {
            _sumOfWeights += pair.Value;
        }

        // ����ȭ ��ųʸ��� ������Ʈ
        UpdateNormalizedDict();
    }

    /// <summary> ����ȭ�� ��ųʸ� ������Ʈ </summary>
    private void UpdateNormalizedDict()
    {
        normalizedItemWeightDict.Clear();
        foreach (var pair in itemWeightDict)
        {
            normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
        }
    }

    /// <summary> �̹� �������� �����ϴ��� ���� �˻� </summary>
    private bool CheckDuplicatedItem(T item)
    {
        if (itemWeightDict.ContainsKey(item))
            return true;
        return false;
    }

    /// <summary> �������� �ʴ� �������� ��� </summary>
    private bool CheckNotExistedItem(T item)
    {
        if (!itemWeightDict.ContainsKey(item))
            return true;
        return false;
    }

    /// <summary> ����ġ �� ���� �˻�(0���� Ŀ�� ��) </summary>
    private bool CheckValidWeight(in double weight)
    {
        if (weight <= 0f)
            return true;
        return false;
    }
    #endregion
}