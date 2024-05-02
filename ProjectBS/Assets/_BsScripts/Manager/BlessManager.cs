using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlessManager : Singleton<BlessManager>
{
    //Resources/UI 폴더에 있는 BlessData를 저장할 딕셔너리
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
        //UIComponent를 상속받은 모든 클래스를 Resources/UI 폴더에서 로드하여 딕셔너리에 저장
        BlessData[] BlessLists = Resources.LoadAll<BlessData>(FilePath.Bless);
        //딕셔너리와 리스트 초기화
        _blessDict = new Dictionary<int, BlessData>();
        callStack = new Stack<UnityAction>();
        blessDataWeight = new WeightedRandomPicker<BlessData>();
        spawnedBless = new Dictionary<int, Bless>();

        //리소스에서 로드한 데이터를 딕셔너리에 저장
        foreach (BlessData data in BlessLists)
        {
            _blessDict.Add(data.ID, data);
        }
        //WeightedRandomPicker에 데이터를 추가
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
    /// BlessData를 기반으로 축복을 생성
    /// </summary>
    public Bless CreateBless(BlessID id)
    {
        //이미 생성되어있는 축복이라면 그것을 반환
        if(spawnedBless.ContainsKey((int)id))
        {
            return spawnedBless[(int)id];
        }
        //딕셔너리에 해당 ID가 존재하면 복제 후 반환
        if (_blessDict.ContainsKey((int)id))
        {
            Bless clone = _blessDict[(int)id].CreateClone();
            return clone;
        }
        //존재하지 않으면 null 반환
        return null;
    }

    //레벨업 시 랜덤으로 선택된 축복를 윈도우에 표시
    public void AppearRandomBlessList()
    {
        //게임 일시정지
        Time.timeScale = 0;
        //윈도우가 활성화 되있을 경우 callStack에 저장 후 리턴
        if(blessSelectWindow.gameObject.activeSelf == true)
        {
            callStack.Push(() => AppearRandomBlessList());
            return;
        }
        //아닐 경우 활성화
        else
            blessSelectWindow.gameObject.SetActive(true);

        temp = new BlessData[3];    //랜덤으로 선택된 축복를 저장할 배열
        weights = new float[3];       //선택된 축복의 가중치를 저장할 배열
        normalizedWeight = new float[3];    //선택된 축복의 정규화된 가중치를 저장할 배열
        //랜덤으로 3개의 축복를 선택
        for (int i = 0; i < 3; i++)
        {
            temp[i] = blessDataWeight.GetRandomPick();                          //WeightedRandomPicker에서 랜덤으로 축복를 선택
            weights[i] = blessDataWeight.GetWeight(temp[i]);                    //선택된 축복의 가중치를 저장
            normalizedWeight[i] = blessDataWeight.GetNormalizedWeight(temp[i]); //선택된 축복의 정규화된 가중치를 저장
            //로그 출력
            Debug.Log($"{temp[i].name}: {weights[i]}, {normalizedWeight[i] * 100.0f}%");
            blessDataWeight.Remove(temp[i]);                                    //선택된 축복를 WeightedRandomPicker에서 제거(중복 선택 방지)
        }

        //윈도우에 축복 이름을 표시(설명 포함)
        string[] names = new string[3];
        for(int i = 0; i < 3; i++)
        {
            if (spawnedBless.ContainsKey(temp[i].ID))
                names[i] = $"{temp[i].Name} Lv:{spawnedBless[temp[i].ID].CurLv + 1}";   //이미 소환되어있는 축복이면 다음 레벨을 표시
            else
                names[i] = $"{temp[i].Name}";                                           //아니면 이름만 표시
        }
        blessSelectWindow.SelectButtons.SetButtonName(names);
        
        //윈도우에 축복 선택 버튼을 추가
        for(int i = 0; i < 3; i++)
        {
            int idx = i;
            blessSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectBless(idx));
        }
    }

    public void SelectBless(int idx)
    {
        //선택된 축복를 WeightedRandomPicker에 추가
        for (int i = 0; i < 3; i++)
        {
            if (i == idx)
            {
                if (temp[idx].ID >= 1500 || temp[idx].ID < 1503)    //선택된 축복이 직업 축복일 경우
                    blessDataWeight.Add(temp[i], 0.5f);             //가중치를 0.5로 설정
                else
                    blessDataWeight.Add(temp[i], weights[i] + 10);  //선택된 축복의 가중치를 증가
            }
            else
                blessDataWeight.Add(temp[i], weights[i]);       //선택되지 않은 축복의 가중치는 그대로
        }
        //소환되어있지 않은 축복이면 생성
        if (!spawnedBless.ContainsKey(temp[idx].ID))
        {
            Bless clone = CreateBless((BlessID)temp[idx].ID);
            spawnedBless.Add(temp[idx].ID, clone);
        }
        //소환되어있는 축복이면 레벨업
        else
        {
            spawnedBless[temp[idx].ID].LevelUp();
        }
        Time.timeScale = 1;
        blessSelectWindow.gameObject.SetActive(false);
        //선택된 축복를 제거한 후 callStack에 저장된 함수 실행
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
    private Dictionary<T, float> normalizedItemWeightDict; // 확률이 정규화된 아이템 목록


    /// <summary> 가중치 합이 계산되지 않은 상태인지 여부 </summary>
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

    /// <summary> 새로운 아이템-가중치 쌍 추가 </summary>
    public void Add(T item, float weight)
    {
        CheckDuplicatedItem(item);
        CheckValidWeight(weight);

        itemWeightDict.Add(item, weight);
        isDirty = true;
    }

    /// <summary> 새로운 아이템-가중치 쌍들 추가 </summary>
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

    /// <summary> 목록에서 대상 아이템 제거 </summary>
    public void Remove(T item)
    {
        CheckNotExistedItem(item);

        itemWeightDict.Remove(item);
        isDirty = true;
    }

    /// <summary> 대상 아이템의 가중치 수정 </summary>
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
    public float GetWeight(T item)
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
    private bool CheckValidWeight(in float weight)
    {
        if (weight <= 0f)
            return true;
        return false;
    }
    #endregion
}