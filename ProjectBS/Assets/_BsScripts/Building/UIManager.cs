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

    //Resources/UI ������ �ִ� UIComponent�� ������ ��ųʸ�
    public Dictionary<int, UIComponent> UIDict => _uiDict;
    private Dictionary<int, UIComponent> _uiDict;
    //Ǯ���� UI�� ID�� ������ ����Ʈ
    private List<int> pooledUIIDList = new List<int>();

    //ĵ������ ���̳��� ĵ������ ������ ����
    [SerializeField]private Transform canvas;
    [SerializeField]private Transform dynamicCanvas;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 2)
        {
            Initialize();
            // �̺�Ʈ �ڵ鷯 ����
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Initialize()
    {
        //UIComponent�� ��ӹ��� ��� Ŭ������ Resources/UI �������� �ε��Ͽ� ��ųʸ��� ����
        UIComponent[] UILists = Resources.LoadAll<UIComponent>(FilePath.UI);
        _uiDict = new Dictionary<int, UIComponent>();
        foreach (UIComponent ui in UILists)
        {
            _uiDict.Add(ui.ID, ui);
        }
        //ĵ������ ���̳��� ĵ������ ���� ��� ����
        
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

    //ĵ���� ���� �Լ�
    private GameObject CreateCanvas(string name)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<Canvas>();
        go.AddComponent<CanvasScaler>();
        go.AddComponent<GraphicRaycaster>();

        return go;
    }

    //ĵ���� �⺻ ���� �Լ�
    private void CanvasSetting(GameObject go)
    {
        go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = go.GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight = 0.5f;
    }

    //Ǯ���� UI�� �����ϴ� �Լ�
    public void SetPool(UIID ID, int max, int init)
    {
        if (!_uiDict.ContainsKey((int)ID))
            return;
        ObjectPoolManager.Instance.SetPool(UIDict[(int)ID], max, init);
        pooledUIIDList.Add((int)ID);
    }

    //Ǯ���� UI�� �������� �Լ�
    public UIComponent GetUI(UIID ID, CanvasType type = CanvasType.DynamicCanvas)
    {
        if (!_uiDict.ContainsKey((int)ID))
        {
            return null;
        }
        //Ǯ���� UI�� ��� ������
        if(pooledUIIDList.Contains((int)ID))
        {
            UIComponent clone = ObjectPoolManager.Instance.GetObj(UIDict[(int)ID]) as UIComponent;
            clone.transform.SetParent(type == CanvasType.Canvas ? canvas : dynamicCanvas);
            return clone;
        }
        //Ǯ������ ���� UI�� ��� ��������
        else
        {
            return CreateUI(ID, type);
        }
    }

    //Ǯ���� UI�� ��ȯ�ϴ� �Լ�
    public void ReleaseUI(UIComponent ui)
    {
        if (!_uiDict.ContainsKey(ui.ID))
        {
            return;
        }
        //Ǯ���� UI�� ��� ��ȯ
        if (pooledUIIDList.Contains(ui.ID))
        {
            ObjectPoolManager.Instance.ReleaseObj(ui);
        }
        //Ǯ������ ���� UI�� ��� �ı�
        else
        {
            Destroy(ui.gameObject);
        }
    }

    //UI�� �����ϴ� �Լ� ������ ĵ���� ����
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