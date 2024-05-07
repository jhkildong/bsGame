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

    //Resources/UI 폴더에 있는 BlessData를 저장할 딕셔너리   <ID, BlessData>
    public Dictionary<int, BlessData> BlessDict => _blessDict;
    private Dictionary<int, BlessData> _blessDict;
    private WeightedRandomPicker<BaseBlessData> blessDataWeight;    //가중치를 적용한 랜덤 선택을 위한 클래스
    private Dictionary<int, Bless> spawnedBless;                    //소환된 축복을 저장할 딕셔너리
    private PassiveBlessData PassiveBlessData;                      //패시브 축복 데이터
    
    private SelectWindow blessSelectWindow;                         //축복 선택 윈도우
    private BlessIconsUI blessIconsUI;                              //현재 소환된 축복을 표시할 UI
    private Stack<UnityAction> callStack;                           //윈도우가 활성화 되어있을 경우 저장할 스택(ex 중복 레벨업)

    BaseBlessData[] temp;           //랜덤으로 선택된 축복를 임시로 저장할 배열
    float[] weights;            //선택된 축복의 가중치를 임시로 저장할 배열
    float[] normalizedWeight;   //선택된 축복의 정규화된 가중치를 임시로 저장할 배열 -> 디버깅용

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
        //UIComponent를 상속받은 모든 클래스를 Resources/UI 폴더에서 로드하여 딕셔너리에 저장
        BlessData[] BlessLists = Resources.LoadAll<BlessData>(FilePath.Bless);
        //딕셔너리와 리스트 초기화
        _blessDict = new Dictionary<int, BlessData>();
        callStack = new Stack<UnityAction>();
        blessDataWeight = new WeightedRandomPicker<BaseBlessData>();
        spawnedBless = new Dictionary<int, Bless>();

        PassiveBlessData = Resources.Load<PassiveBlessData>(FilePath.PassiveBless);

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
        //패시브 축복의 가중치를 추가
        float sum = blessDataWeight.SumOfWeights;
        float passiveWeight = sum * 0.111111f;  //패시브 축복의 가중치를 1/9로 설정(전체가중치 7, 패시브가중치 3)
        blessDataWeight.Add(PassiveBlessData, passiveWeight);
    }

    private void ModifyPassiveBlessWeight()
    {
        float sum = blessDataWeight.SumOfWeights;
        float passiveWeight = blessDataWeight.GetWeight(PassiveBlessData);
        passiveWeight = (sum - passiveWeight)  * 0.111111f;  //패시브 축복의 가중치를 3/7로 설정(전체가중치 7, 패시브가중치 3)
        blessDataWeight.ModifyWeight(PassiveBlessData, passiveWeight);
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
            spawnedBless.Add(_blessDict[(int)id].ID, clone);

            return clone;
        }
        //존재하지 않으면 null 반환
        return null;
    }

    //레벨업 시 랜덤으로 선택된 축복를 윈도우에 표시
    public void AppearRandomBlessList()
    {
        //게임 일시정지
        GameManager.Instance.PauseGame();
        //윈도우가 활성화 되있을 경우 callStack에 저장 후 리턴
        if(blessSelectWindow.gameObject.activeSelf == true)
        {
            callStack.Push(() => AppearRandomBlessList());
            return;
        }
        //아닐 경우 활성화
        else
            blessSelectWindow.gameObject.SetActive(true);

        temp = new BaseBlessData[3];        //랜덤으로 선택된 축복를 저장할 배열
        weights = new float[3];             //선택된 축복의 가중치를 저장할 배열
        normalizedWeight = new float[3];    //선택된 축복의 정규화된 가중치를 저장할 배열
        //랜덤으로 3개의 축복를 선택
        for (int i = 0; i < 3; i++)
        {
            if(blessDataWeight.GetRandomPick() is BlessData blessData)                  //선택된 축복이 BlessData일 경우
            {
                temp[i] = blessData;                                                    //임시 배열에 저장
                weights[i] = blessDataWeight.GetWeight(blessData);                      //선택된 축복의 가중치를 저장
                normalizedWeight[i] = blessDataWeight.GetNormalizedWeight(blessData);   //선택된 축복의 정규화된 가중치를 저장
                blessDataWeight.Remove(temp[i]);                                        //선택된 축복를 WeightedRandomPicker에서 제거(중복 선택 방지)
                ModifyPassiveBlessWeight();                                             //패시브 축복의 가중치를 조정(항상 30%로 유지)
            }
            else if(PassiveBlessData is PassiveBlessData passiveData)                   //선택된 축복이 PassiveBlessData일 경우
            {
                temp[i] = passiveData;                                                    //임시 배열에 저장
                weights[i] = blessDataWeight.GetWeight(passiveData);                      //선택된 축복의 가중치를 저장
                normalizedWeight[i] = blessDataWeight.GetNormalizedWeight(passiveData);   //선택된 축복의 정규화된 가중치를 저장
            }
#if UNITY_EDITOR
            //로그 출력
            Debug.Log($"{temp[i].name}: {weights[i]}, {normalizedWeight[i] * 100.0f}%");
#endif
        }

        //윈도우에 축복 이름을 표시(설명 포함)
        string[] names = new string[3];
        string[] description = new string[3];
        Sprite[] sprites = new Sprite[3];
        List<int> exclude = new List<int>();
        for(int i = 0; i < 3; i++)
        {
            int idx = i;
            blessSelectWindow.SelectButtons.SetButtonAction(idx, () => SelectBless(idx));
            if (temp[i] is BlessData blessData)                         //선택된 축복이 BlessData일 경우
            {
                if (spawnedBless.ContainsKey(blessData.ID))
                {
                    int CurLv = spawnedBless[blessData.ID].CurLv;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Lv.");
                    sb.Append(CurLv + 1);                   //다음레벨을 표시
                    sb.Append("\n");
                    sb.Append(blessData.Name);              //이름을 표시
                    names[i] = sb.ToString();
                    description[i] = LevelUpDescription.DescriptionDic[blessData.ID][CurLv];    //설명을 표시 인덱스이기 때문에 현재 레벨로 접근
                    sprites[i] = blessData.Icon; //이미 소환되어있는 축복이면 다음 레벨을 표시
                    blessSelectWindow.SelectButtons.shinies[i].Play();
                    if (CurLv >= 6)
                    {   //현재 레벨이 6이고 직업 축복일 경우
                        if (blessData.ID >= (int)BlessID.WARRIOR && blessData.ID <= (int)BlessID.MAGE)
                        {
                            Color color = new Color(255, 223, 0, 255);
                            blessSelectWindow.SelectButtons.SetShinyColor(color, i);
                        }
                    }
                    
                }
                else
                {
                    names[i] = blessData.Name;
                    description[i] = blessData.Description;
                    sprites[i] = blessData.Icon;  //아니면 이름만 표시
                }
            }
            else if (temp[i] is PassiveBlessData passiveData)           //선택된 축복이 PassiveBlessData일 경우
            {
                //중복된 패시브가 선택되지 않게 저장
                exclude.Add(passiveData.ShowRandomPassive(out string desc, out UnityAction action, exclude.ToArray()));
                blessSelectWindow.SelectButtons.AddButtonAction(idx, action);
                names[i] = "강화의 축복";
                description[i] = desc;
                sprites[i] = passiveData.Icon;
            }
        }

        blessSelectWindow.SelectButtons.SetNames(names);
        blessSelectWindow.SelectButtons.SetDecriptions(description);
        blessSelectWindow.SelectButtons.SetImages(sprites);

        
        if(RerollCount > 0)
        {
            blessSelectWindow.SetUndoButtonInteract(true);
            blessSelectWindow.SetUndoButtonText($"{RerollCount}");
            blessSelectWindow.SetUndoButtonAct(() =>
            {
                RerollCount--;
                for (int i = 0; i < 3; i++)
                {
                    if (temp[i] is PassiveBlessData)    //패시브 축복이면 continue(패시브 축복은 꺼내지 않음)
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
        //선택된 축복를 다시 WeightedRandomPicker에 넣기
        for (int i = 0; i < 3; i++)
        {
            if (temp[i] is PassiveBlessData)    //패시브 축복이면 continue(패시브 축복은 꺼내지 않음)
                continue;
            if (i == idx)
            {
                if ((temp[idx].ID >= (int)BlessID.WARRIOR) && temp[idx].ID <= (int)BlessID.MAGE) //선택된 축복이 직업 축복이고 레벨 6일경우
                {
                    if (spawnedBless[temp[idx].ID].CurLv == 5)          //레벨 5 -> 6으로 갈때 설정해둬야 레벨 6 -> 7 갈때 적용이 됨
                    {
                        blessDataWeight.Add(temp[i], 0.2f);             //가중치를 0.5로 설정
                        continue;
                    }
                }
                blessDataWeight.Add(temp[i], weights[i] + 3);  //선택된 축복의 가중치를 증가
            }
            else
                blessDataWeight.Add(temp[i], weights[i]);       //선택되지 않은 축복의 가중치는 그대로
        }
        ModifyPassiveBlessWeight();                             //패시브 축복의 가중치를 조정(다음 선택지에서도 30%로 유지 되게)

        if (temp[idx] is BlessData blessData)                    //선택된 축복이 BlessData일 경우
        {
            //소환되어있지 않은 축복이면 생성
            if (!spawnedBless.ContainsKey(blessData.ID))
            {
                CreateBless((BlessID)blessData.ID);
                blessIconsUI.AddBlessIcon(blessData);
            }
            //소환되어있는 축복이면 레벨업
            else
            {
                spawnedBless[blessData.ID].LevelUp();
                string Lv = $"Lv.{spawnedBless[blessData.ID].CurLv}";
                blessIconsUI.SetText(Lv, blessData.ID);
            }
        }
        blessSelectWindow.gameObject.SetActive(false);
        //축복 선택이 끝난 후 callStack에 저장된 함수가 있으면 실행
        if (callStack.Count > 0)
        {
            callStack.Pop().Invoke();
        }
        else
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
        Random.InitState((int)(System.DateTime.Now.Ticks % int.MaxValue));
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