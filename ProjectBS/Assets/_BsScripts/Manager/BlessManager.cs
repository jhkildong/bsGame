using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlessManager : Singleton<BlessManager>
{
    //Resources/UI ������ �ִ� BlessData�� ������ ��ųʸ�
    public Dictionary<int, BlessData> BlessDict => _blessDict;
    
    private Dictionary<int, BlessData> _blessDict;
    private WeightedRandomPicker<BlessData> blessDataWeight;
    private SelectWindow blessSelectWindow;
    private Stack<UnityAction> callStack;
    private Dictionary<int, Bless> spawnedBless;

    BlessData[] temp;
    float[] weights;
    float[] normalizedWeight;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Start()
    {
        blessSelectWindow = UIManager.Instance.CreateUI(UIID.BlessSelectWindow, CanvasType.Canvas) as SelectWindow;
        blessSelectWindow.gameObject.SetActive(false);
    }


    private void Initialize()
    {
        //UIComponent�� ��ӹ��� ��� Ŭ������ Resources/UI �������� �ε��Ͽ� ��ųʸ��� ����
        BlessData[] BlessLists = Resources.LoadAll<BlessData>(FilePath.Bless);
        //��ųʸ��� ����Ʈ �ʱ�ȭ
        _blessDict = new Dictionary<int, BlessData>();
        callStack = new Stack<UnityAction>();
        blessDataWeight = new WeightedRandomPicker<BlessData>();
        spawnedBless = new Dictionary<int, Bless>();

        //���ҽ����� �ε��� �����͸� ��ųʸ��� ����
        foreach (BlessData data in BlessLists)
        {
            _blessDict.Add(data.ID, data);
        }
        //WeightedRandomPicker�� �����͸� �߰�
        foreach(var item in _blessDict)
        {
            blessDataWeight.Add(item.Value, 1);
        }
        for(int i = 1500; i < 1503; i++)
        {
            spawnedBless.Add(i, CreateBless((BlessID)i));
        }
    }
    #region Public Method
    /// <summary>
    /// BlessData�� ������� �ູ�� ����
    /// </summary>
    public Bless CreateBless(BlessID id)
    {
        //�̹� �����Ǿ��ִ� �ູ�̶�� �װ��� ��ȯ
        if(spawnedBless.ContainsKey((int)id))
        {
            return spawnedBless[(int)id];
        }
        //��ųʸ��� �ش� ID�� �����ϸ� ���� �� ��ȯ
        if (_blessDict.ContainsKey((int)id))
        {
            Bless clone = _blessDict[(int)id].CreateClone();
            return clone;
        }
        //�������� ������ null ��ȯ
        return null;
    }

    //������ �� �������� ���õ� �ູ�� �����쿡 ǥ��
    public void AppearRandomBlessList()
    {
        //���� �Ͻ�����
        Time.timeScale = 0;
        //�����찡 Ȱ��ȭ ������ ��� callStack�� ���� �� ����
        if(blessSelectWindow.gameObject.activeSelf == true)
        {
            callStack.Push(() => AppearRandomBlessList());
            return;
        }
        //�ƴ� ��� Ȱ��ȭ
        else
            blessSelectWindow.gameObject.SetActive(true);

        temp = new BlessData[3];    //�������� ���õ� �ູ�� ������ �迭
        weights = new float[3];       //���õ� �ູ�� ����ġ�� ������ �迭
        normalizedWeight = new float[3];    //���õ� �ູ�� ����ȭ�� ����ġ�� ������ �迭
        //�������� 3���� �ູ�� ����
        for (int i = 0; i < 3; i++)
        {
            temp[i] = blessDataWeight.GetRandomPick();                          //WeightedRandomPicker���� �������� �ູ�� ����
            weights[i] = blessDataWeight.GetWeight(temp[i]);                    //���õ� �ູ�� ����ġ�� ����
            normalizedWeight[i] = blessDataWeight.GetNormalizedWeight(temp[i]); //���õ� �ູ�� ����ȭ�� ����ġ�� ����
            //�α� ���
            Debug.Log($"{temp[i].name}: {weights[i]}, {normalizedWeight[i] * 100.0f}%");
            blessDataWeight.Remove(temp[i]);                                    //���õ� �ູ�� WeightedRandomPicker���� ����(�ߺ� ���� ����)
        }

        //�����쿡 �ູ �̸��� ǥ��(���� ����)
        string[] names = new string[3];
        for(int i = 0; i < 3; i++)
        {
            if (spawnedBless.ContainsKey(temp[i].ID))
                names[i] = $"{temp[i].Name} Lv:{spawnedBless[temp[i].ID].CurLv + 1}";   //�̹� ��ȯ�Ǿ��ִ� �ູ�̸� ���� ������ ǥ��
            else
                names[i] = $"{temp[i].Name}";                                           //�ƴϸ� �̸��� ǥ��
        }
        blessSelectWindow.SelectButtons.SetButtonName(names);
        
        //�����쿡 �ູ ���� ��ư�� �߰�
        for(int i = 0; i < 3; i++)
        {
            int idx = i;
            blessSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectBless(idx));
        }
    }

    public void SelectBless(int idx)
    {
        //���õ� �ູ�� WeightedRandomPicker�� �߰�
        for (int i = 0; i < 3; i++)
        {
            if (i == idx)
            {
                if (temp[idx].ID >= 1500 || temp[idx].ID < 1503)    //���õ� �ູ�� ���� �ູ�� ���
                    blessDataWeight.Add(temp[i], 0.5f);             //����ġ�� 0.5�� ����
                else
                    blessDataWeight.Add(temp[i], weights[i] + 10);  //���õ� �ູ�� ����ġ�� ����
            }
            else
                blessDataWeight.Add(temp[i], weights[i]);       //���õ��� ���� �ູ�� ����ġ�� �״��
        }
        //��ȯ�Ǿ����� ���� �ູ�̸� ����
        if (!spawnedBless.ContainsKey(temp[idx].ID))
        {
            Bless clone = CreateBless((BlessID)temp[idx].ID);
            spawnedBless.Add(temp[idx].ID, clone);
        }
        //��ȯ�Ǿ��ִ� �ູ�̸� ������
        else
        {
            spawnedBless[temp[idx].ID].LevelUp();
        }
        Time.timeScale = 1;
        blessSelectWindow.gameObject.SetActive(false);
        //���õ� �ູ�� ������ �� callStack�� ����� �Լ� ����
        if (callStack.Count > 0)
        {
            callStack.Pop().Invoke();
        }
    }

    public void FinishLevelUp(int ID)
    {
        blessDataWeight.Remove(BlessDict[ID]);
    }
    #endregion
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
    public float SumOfWeights
    {
        get
        {
            CalculateSumIfDirty();
            return _sumOfWeights;
        }
    }

    private Dictionary<T, float> itemWeightDict;
    private Dictionary<T, float> normalizedItemWeightDict; // Ȯ���� ����ȭ�� ������ ���


    /// <summary> ����ġ ���� ������ ���� �������� ���� </summary>
    private bool isDirty;
    private float _sumOfWeights;

    /***********************************************************************
    *                               Constructors
    ***********************************************************************/
    #region .
    public WeightedRandomPicker()
    {
        itemWeightDict = new Dictionary<T, float>();
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
    public void Add(T item, float weight)
    {
        CheckDuplicatedItem(item);
        CheckValidWeight(weight);

        itemWeightDict.Add(item, weight);
        isDirty = true;
    }

    /// <summary> ���ο� ������-����ġ �ֵ� �߰� </summary>
    public void Add(params (T item, float weight)[] pairs)
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
    public void ModifyWeight(T item, float weight)
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
    public float GetWeight(T item)
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
    private bool CheckValidWeight(in float weight)
    {
        if (weight <= 0f)
            return true;
        return false;
    }
    #endregion
}