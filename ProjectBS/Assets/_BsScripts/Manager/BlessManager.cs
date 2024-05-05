using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BlessManager : Singleton<BlessManager>
{
    public int RerollCount { get => _rerollCount; set => _rerollCount = value; }
    [SerializeField]private int _rerollCount;

    //Resources/UI ������ �ִ� BlessData�� ������ ��ųʸ�   <ID, BlessData>
    public Dictionary<int, BlessData> BlessDict => _blessDict;
    private Dictionary<int, BlessData> _blessDict;
    private WeightedRandomPicker<BaseBlessData> blessDataWeight;    //����ġ�� ������ ���� ������ ���� Ŭ����
    private Dictionary<int, Bless> spawnedBless;                    //��ȯ�� �ູ�� ������ ��ųʸ�
    private PassiveBlessData PassiveBlessData;                      //�нú� �ູ ������
    
    private SelectWindow blessSelectWindow;                         //�ູ ���� ������
    private BlessIconsUI blessIconsUI;                              //���� ��ȯ�� �ູ�� ǥ���� UI
    private Stack<UnityAction> callStack;                           //�����찡 Ȱ��ȭ �Ǿ����� ��� ������ ����(ex �ߺ� ������)

    BaseBlessData[] temp;           //�������� ���õ� �ູ�� �ӽ÷� ������ �迭
    float[] weights;            //���õ� �ູ�� ����ġ�� �ӽ÷� ������ �迭
    float[] normalizedWeight;   //���õ� �ູ�� ����ȭ�� ����ġ�� �ӽ÷� ������ �迭 -> ������

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void OnEnable()
    {
        blessSelectWindow = UIManager.Instance.CreateUI(UIID.BlessSelectWindow, CanvasType.Canvas) as SelectWindow;
        blessSelectWindow.gameObject.SetActive(false);
        blessIconsUI = UIManager.Instance.CreateUI(UIID.BlessIconsUI, CanvasType.Canvas) as BlessIconsUI;
        LevelUpDescription.GetLevelUpDescriptionToJson();
    }

    private void Initialize()
    {
        //UIComponent�� ��ӹ��� ��� Ŭ������ Resources/UI �������� �ε��Ͽ� ��ųʸ��� ����
        BlessData[] BlessLists = Resources.LoadAll<BlessData>(FilePath.Bless);
        //��ųʸ��� ����Ʈ �ʱ�ȭ
        _blessDict = new Dictionary<int, BlessData>();
        callStack = new Stack<UnityAction>();
        blessDataWeight = new WeightedRandomPicker<BaseBlessData>();
        spawnedBless = new Dictionary<int, Bless>();

        PassiveBlessData = Resources.Load<PassiveBlessData>(FilePath.PassiveBless);

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
        //�нú� �ູ�� ����ġ�� �߰�
        float sum = blessDataWeight.SumOfWeights;
        float passiveWeight = sum * 0.111111f;  //�нú� �ູ�� ����ġ�� 1/9�� ����(��ü����ġ 7, �нú갡��ġ 3)
        blessDataWeight.Add(PassiveBlessData, passiveWeight);
    }

    private void ModifyPassiveBlessWeight()
    {
        float sum = blessDataWeight.SumOfWeights;
        float passiveWeight = blessDataWeight.GetWeight(PassiveBlessData);
        passiveWeight = (sum - passiveWeight)  * 0.111111f;  //�нú� �ູ�� ����ġ�� 3/7�� ����(��ü����ġ 7, �нú갡��ġ 3)
        blessDataWeight.ModifyWeight(PassiveBlessData, passiveWeight);
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
            spawnedBless.Add(_blessDict[(int)id].ID, clone);

            return clone;
        }
        //�������� ������ null ��ȯ
        return null;
    }

    //������ �� �������� ���õ� �ູ�� �����쿡 ǥ��
    public void AppearRandomBlessList()
    {
        //���� �Ͻ�����
        GameManager.Instance.PauseGame();
        //�����찡 Ȱ��ȭ ������ ��� callStack�� ���� �� ����
        if(blessSelectWindow.gameObject.activeSelf == true)
        {
            callStack.Push(() => AppearRandomBlessList());
            return;
        }
        //�ƴ� ��� Ȱ��ȭ
        else
            blessSelectWindow.gameObject.SetActive(true);

        temp = new BaseBlessData[3];        //�������� ���õ� �ູ�� ������ �迭
        weights = new float[3];             //���õ� �ູ�� ����ġ�� ������ �迭
        normalizedWeight = new float[3];    //���õ� �ູ�� ����ȭ�� ����ġ�� ������ �迭
        //�������� 3���� �ູ�� ����
        for (int i = 0; i < 3; i++)
        {
            if(blessDataWeight.GetRandomPick() is BlessData blessData)                  //���õ� �ູ�� BlessData�� ���
            {
                temp[i] = blessData;                                                    //�ӽ� �迭�� ����
                weights[i] = blessDataWeight.GetWeight(blessData);                      //���õ� �ູ�� ����ġ�� ����
                normalizedWeight[i] = blessDataWeight.GetNormalizedWeight(blessData);   //���õ� �ູ�� ����ȭ�� ����ġ�� ����
                blessDataWeight.Remove(temp[i]);                                        //���õ� �ູ�� WeightedRandomPicker���� ����(�ߺ� ���� ����)
                ModifyPassiveBlessWeight();                                             //�нú� �ູ�� ����ġ�� ����(�׻� 30%�� ����)
            }
            else if(PassiveBlessData is PassiveBlessData passiveData)                   //���õ� �ູ�� PassiveBlessData�� ���
            {
                temp[i] = passiveData;                                                    //�ӽ� �迭�� ����
                weights[i] = blessDataWeight.GetWeight(passiveData);                      //���õ� �ູ�� ����ġ�� ����
                normalizedWeight[i] = blessDataWeight.GetNormalizedWeight(passiveData);   //���õ� �ູ�� ����ȭ�� ����ġ�� ����
            }
#if UNITY_EDITOR
            //�α� ���
            Debug.Log($"{temp[i].name}: {weights[i]}, {normalizedWeight[i] * 100.0f}%");
#endif
        }

        //�����쿡 �ູ �̸��� ǥ��(���� ����)
        string[] names = new string[3];
        List<int> exclude = new List<int>();
        for(int i = 0; i < 3; i++)
        {
            int idx = i;
            blessSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectBless(idx));
            if (temp[i] is BlessData blessData)                         //���õ� �ູ�� BlessData�� ���
            {
                StringBuilder sb = new StringBuilder();
                if (spawnedBless.ContainsKey(blessData.ID))
                {
                    int CurLv = spawnedBless[blessData.ID].CurLv;
                    sb.Append(blessData.Name);
                    sb.Append(" Lv.");
                    sb.Append(CurLv + 1);
                    sb.Append(" : ");
                    sb.Append(LevelUpDescription.DescriptionDic[blessData.ID][CurLv]);
                    names[i] = sb.ToString();   //�̹� ��ȯ�Ǿ��ִ� �ູ�̸� ���� ������ ǥ��
                }
                else
                {
                    sb.Append(blessData.Name);
                    sb.Append(" : ");
                    sb.Append(blessData.Description);
                    names[i] = sb.ToString();   //�ƴϸ� �̸��� ǥ��
                }
            }
            else if (temp[i] is PassiveBlessData passiveData)           //���õ� �ູ�� PassiveBlessData�� ���
            {
                exclude.Add(passiveData.ShowRandomPassive(out string desc, out UnityAction action, exclude.ToArray()));
                blessSelectWindow.SelectButtons.AddButtonAction(idx, action);
                names[i] = desc;
            }
        }

        blessSelectWindow.SelectButtons.SetButtonName(names);

        if(RerollCount > 0)
        {
            blessSelectWindow.SetUndoButtonInteract(true);
            blessSelectWindow.SetUndoButtonText($"{RerollCount}");
            blessSelectWindow.SetUndoButtonAct(() =>
            {
                RerollCount--;
                for (int i = 0; i < 3; i++)
                {
                    if (temp[i] is PassiveBlessData)    //�нú� �ູ�̸� continue(�нú� �ູ�� ������ ����)
                        continue;
                    blessDataWeight.Add(temp[i], weights[i]);
                }
                ModifyPassiveBlessWeight();
                blessSelectWindow.gameObject.SetActive(false);
                AppearRandomBlessList();
            });
        }
        else
        {
            blessSelectWindow.SetUndoButtonText($"{RerollCount}");
            blessSelectWindow.SetUndoButtonInteract(false);
        }

    }

    private void SelectBless(int idx)
    {
        //���õ� �ູ�� �ٽ� WeightedRandomPicker�� �ֱ�
        for (int i = 0; i < 3; i++)
        {
            if (temp[i] is PassiveBlessData)    //�нú� �ູ�̸� continue(�нú� �ູ�� ������ ����)
                continue;
            if (i == idx)
            {
                if ((temp[idx].ID >= (int)BlessID.WARRIOR) && temp[idx].ID <= (int)BlessID.MAGE) //���õ� �ູ�� ���� �ູ�̰� ���� 6�ϰ��
                {
                    if (spawnedBless[temp[idx].ID].CurLv == 5)          //���� 5 -> 6���� ���� �����ص־� ���� 6 -> 7 ���� ������ ��
                    {
                        blessDataWeight.Add(temp[i], 0.2f);             //����ġ�� 0.5�� ����
                        continue;
                    }
                }
                blessDataWeight.Add(temp[i], weights[i] + 3);  //���õ� �ູ�� ����ġ�� ����
            }
            else
                blessDataWeight.Add(temp[i], weights[i]);       //���õ��� ���� �ູ�� ����ġ�� �״��
        }
        ModifyPassiveBlessWeight();                             //�нú� �ູ�� ����ġ�� ����(���� ������������ 30%�� ���� �ǰ�)

        if (temp[idx] is BlessData blessData)                    //���õ� �ູ�� BlessData�� ���
        {
            //��ȯ�Ǿ����� ���� �ູ�̸� ����
            if (!spawnedBless.ContainsKey(blessData.ID))
            {
                CreateBless((BlessID)blessData.ID);
                blessIconsUI.AddBlessIcon(blessData);
            }
            //��ȯ�Ǿ��ִ� �ູ�̸� ������
            else
            {
                spawnedBless[blessData.ID].LevelUp();
                string Lv = $"Lv.{spawnedBless[blessData.ID].CurLv}";
                blessIconsUI.SetText(Lv, blessData.ID);
            }
        }
        blessSelectWindow.gameObject.SetActive(false);
        //�ູ ������ ���� �� callStack�� ����� �Լ��� ������ ����
        if (callStack.Count > 0)
        {
            callStack.Pop().Invoke();
        }
        GameManager.Instance.ResumeGame();
    }

    public void SetJobBlessIcon(int ID)
    {
        blessIconsUI.SetJobBlessIcon(BlessDict[ID]);
    }

    public void RemoveBlessInSelectPool(int ID)
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
    WARRIOR = 1500, ARCHER = 1501, MAGE = 1502

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
        Random.InitState((int)(System.DateTime.Now.Ticks % int.MaxValue));
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