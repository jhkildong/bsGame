using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeon2;

public class BlessManager : Singleton<BlessManager>
{
    //Resources/UI 폴더에 있는 BlessData를 저장할 딕셔너리
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
        //UIComponent를 상속받은 모든 클래스를 Resources/UI 폴더에서 로드하여 딕셔너리에 저장
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
        //참조하는 윈도우가 없을경우 생성
        if(blessSelectWindow == null)
        {
            blessSelectWindow = UIManager.Instance.CreateUI(UIID.BlessSelectWindow, CanvasType.Canvas) as SelectWindow;
        }
        //윈도우가 활성화 되있을 경우 callStack에 저장
        if(blessSelectWindow.gameObject.activeSelf == true)
            callStack.Push(() => AppearRandomBlessList());
        //아닐 경우 활성화
        else
            blessSelectWindow.gameObject.SetActive(true);

        temp = new BlessData[3];
        weights = new int[3];
        //랜덤으로 3개의 블레스를 선택
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
        //선택된 블레스를 플레이어에게 부여
        CreateBless((BlessID)temp[idx].ID);
        //선택된 블레스를 WeightedRandomPicker에 추가
        for(int i = 0; i < 3; i++)
        {
            if(i == idx)
                blessDataWeight.Add(temp[i], weights[i] + 10);
            else
                blessDataWeight.Add(temp[i], weights[i]);
        }
        //선택된 블레스를 제거한 후 callStack에 저장된 함수 실행
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
    private Dictionary<T, float> normalizedItemWeightDict; // 확률이 정규화된 아이템 목록


    /// <summary> 가중치 합이 계산되지 않은 상태인지 여부 </summary>
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

    /// <summary> 새로운 아이템-가중치 쌍 추가 </summary>
    public void Add(T item, int weight)
    {
        CheckDuplicatedItem(item);
        CheckValidWeight(weight);

        itemWeightDict.Add(item, weight);
        isDirty = true;
    }

    /// <summary> 새로운 아이템-가중치 쌍들 추가 </summary>
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

    /// <summary> 목록에서 대상 아이템 제거 </summary>
    public void Remove(T item)
    {
        CheckNotExistedItem(item);

        itemWeightDict.Remove(item);
        isDirty = true;
    }

    /// <summary> 대상 아이템의 가중치 수정 </summary>
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

    /// <summary> 랜덤 뽑기 </summary>
    public T GetRandomPick()
    {
        // 랜덤 계산
        float chance = Random.value; // [0.0, 1.0)
        chance *= SumOfWeights;

        return GetRandomPick(chance);
    }

    /// <summary> 직접 랜덤 값을 지정하여 뽑기 </summary>
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

        //여기까지 왔다면 뭔가 잘못된 것
        return default;
    }

    /// <summary> 대상 아이템의 가중치 확인 </summary>
    public int GetWeight(T item)
    {
        return itemWeightDict[item];
    }

    /// <summary> 대상 아이템의 정규화된 가중치 확인 </summary>
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
    /// <summary> 모든 아이템의 가중치 합 계산해놓기 </summary>
    private void CalculateSumIfDirty()
    {
        if (!isDirty) return;
        isDirty = false;

        _sumOfWeights = 0;
        foreach (var pair in itemWeightDict)
        {
            _sumOfWeights += pair.Value;
        }

        // 정규화 딕셔너리도 업데이트
        UpdateNormalizedDict();
    }

    /// <summary> 정규화된 딕셔너리 업데이트 </summary>
    private void UpdateNormalizedDict()
    {
        normalizedItemWeightDict.Clear();
        foreach (var pair in itemWeightDict)
        {
            normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
        }
    }

    /// <summary> 이미 아이템이 존재하는지 여부 검사 </summary>
    private bool CheckDuplicatedItem(T item)
    {
        if (itemWeightDict.ContainsKey(item))
            return true;
        return false;
    }

    /// <summary> 존재하지 않는 아이템인 경우 </summary>
    private bool CheckNotExistedItem(T item)
    {
        if (!itemWeightDict.ContainsKey(item))
            return true;
        return false;
    }

    /// <summary> 가중치 값 범위 검사(0보다 커야 함) </summary>
    private bool CheckValidWeight(in double weight)
    {
        if (weight <= 0f)
            return true;
        return false;
    }
    #endregion
}