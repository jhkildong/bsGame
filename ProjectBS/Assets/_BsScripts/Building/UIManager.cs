using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Resources/UI 폴더에 있는 UIComponent를 저장할 딕셔너리
    public Dictionary<int, UIComponent> UIDict => _uiDict;
    private Dictionary<int, UIComponent> _uiDict;
    //풀링된 UI의 ID를 저장할 리스트
    private List<int> pooledUIIDList = new List<int>();

    //캔버스와 다이나믹 캔버스를 저장할 변수
    [SerializeField]private Transform canvas;
    [SerializeField]private Transform dynamicCanvas;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 2)
        {
            Initialize();
            // 이벤트 핸들러 제거
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Initialize()
    {
        //UIComponent를 상속받은 모든 클래스를 Resources/UI 폴더에서 로드하여 딕셔너리에 저장
        UIComponent[] UILists = Resources.LoadAll<UIComponent>(FilePath.UI);
        _uiDict = new Dictionary<int, UIComponent>();
        foreach (UIComponent ui in UILists)
        {
            _uiDict.Add(ui.ID, ui);
        }
        //캔버스와 다이나믹 캔버스가 없을 경우 생성
        
        GameObject _canvas = GameObject.Find("Canvas");
        GameObject _dynamicCanvas = GameObject.Find("DynamicCanvas");
        if (_canvas == null)
            canvas = CreateCanvas("Canvas").transform;
        else
            canvas = _canvas.transform;
        if (dynamicCanvas == null)
            dynamicCanvas = CreateCanvas("DynamicCanvas").transform;
        else
            dynamicCanvas = _dynamicCanvas.transform;
        CanvasSetting(canvas.gameObject);
        CanvasSetting(dynamicCanvas.gameObject);
    }

    //캔버스 생성 함수
    private GameObject CreateCanvas(string name)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<Canvas>();
        go.AddComponent<CanvasScaler>();
        go.AddComponent<GraphicRaycaster>();

        return go;
    }

    //캔버스 기본 설정 함수
    private void CanvasSetting(GameObject go)
    {
        go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = go.GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 0.5f;
    }

    //풀링할 UI를 설정하는 함수
    public void SetPool(UIID ID, int max, int init)
    {
        if (!_uiDict.ContainsKey((int)ID))
            return;
        ObjectPoolManager.Instance.SetPool(UIDict[(int)ID], max, init);
        pooledUIIDList.Add((int)ID);
    }

    //풀링한 UI를 가져오는 함수
    public UIComponent GetUI(UIID ID, CanvasType type = CanvasType.DynamicCanvas)
    {
        if (!_uiDict.ContainsKey((int)ID))
        {
            return null;
        }
        //풀링된 UI일 경우 가져옴
        if(pooledUIIDList.Contains((int)ID))
        {
            UIComponent clone = ObjectPoolManager.Instance.GetObj(UIDict[(int)ID]) as UIComponent;
            clone.transform.SetParent(type == CanvasType.Canvas ? canvas : dynamicCanvas);
            return clone;
        }
        //풀링되지 않은 UI일 경우 생성해줌
        else
        {
            return CreateUI(ID, type);
        }
    }

    //풀링한 UI를 반환하는 함수
    public void ReleaseUI(UIComponent ui)
    {
        if (!_uiDict.ContainsKey(ui.ID))
        {
            return;
        }
        //풀링된 UI일 경우 반환
        if (pooledUIIDList.Contains(ui.ID))
        {
            ObjectPoolManager.Instance.ReleaseObj(ui);
        }
        //풀링되지 않은 UI일 경우 파괴
        else
        {
            Destroy(ui.gameObject);
        }
    }

    //UI를 생성하는 함수 생성시 캔버스 결정
    public UIComponent CreateUI(UIID ID, CanvasType type)
    {
        if (!_uiDict.ContainsKey((int)ID))
            return null;
        return Instantiate(UIDict[(int)ID], type == CanvasType.Canvas ? canvas : dynamicCanvas);
    }

    public void testcode(GameObject prefab, Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }
}

public enum CanvasType
{
    Canvas = 0,
    DynamicCanvas = 1,
}

public enum UIID
{
    BlessPopup = 5000,
    PlayerUI = 5001,
    PlayerSelectWindow = 5002,
    BuildingPopup = 5003,
    ConstructionKeyUI = 5004,
    ProgressBar = 5005,
    DamageUI = 5006,
    BlessSelectWindow = 5007,
    HealFontUI = 5008,
    BuildingHpBar = 5010,
    BuildingInteractionUI = 5011,
    SkillIconUI = 5012,
    BlessIconsUI = 5013,
    CreditsUI = 5014,
    SettingsUI = 5015,
    ShopUI = 5016,
    GameOverUI = 5099
}