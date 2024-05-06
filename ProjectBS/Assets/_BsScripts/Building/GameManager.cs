using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
using Unity.Burst.CompilerServices;
using System;

public class GameManager : MonoBehaviour
{
    //�̱��� ������ ����ϱ� ���� �ν��Ͻ� ����. �̷��� �����ϸ� ��ü�� �������� �ʾƵ� static�̱� ������ �ܺ� ��𼭵� ���� ����

    //GameManager.Instance.StartGame(); �ܺο��� �̷� ������ ���� �����ϴ�.

    private static GameManager _inst;

    public Player Player;
    public Light Sunlight;
    public TextMeshProUGUI curSecText;//���� �ð�(��) �ؽ�Ʈ (player Ui�� ����)
    public TextMeshProUGUI curMinText;//���� �ð�(��) �ؽ�Ʈ (player Ui�� ����)

    [HideInInspector]public PlayerComponent myJob;
    [HideInInspector]public int myTypeIdx;

    public void SetSelectedInfo(PlayerComponent job, int typeIdx)
    {
        myJob = job;
        myTypeIdx = typeIdx;
    }

    public bool gameStart;
    public bool gameOver;
    public bool gamePaused;
    public float curSec; // ����ð�(��)
    public float curMin; // ����ð�(��)
    public float curTime; // ���� �ð�
    public float checkAMPMTime;
    private bool isNight;
    public int curDay;
    private int curGold;
    //private int myExp;
    private int myWood = 0;
    private int myStone = 0;
    private int myIron = 0;
    private int myGold = 0;

    private int myLevel = 1; // �� ����
    //private int myExp; //����ġ �ѷ�
    private int curLvExp; //���緹�� ���� ����ġ
    public int CurLvExp
    {
        get { return curLvExp; }
    }
    private int requireExp; // �������� �ʿ��� ����ġ �ѷ�
    public int RequireExp
    {
        get { return requireExp; }
    }
    private int overExp; //�������� ��ģ ����ġ
    public event UnityAction<int> WoodChangeAct;
    public event UnityAction<int> StoneChangeAct;
    public event UnityAction<int> IronChangeAct;
    public event UnityAction<int> GoldChangeAct;
    public event UnityAction<int> ExpChangeAct;
    public event UnityAction<int> ChangeDayAct;
    public event UnityAction<bool> ChangeAMPMAct;


    private PlayerUI playerUI;


    private int CalcRequireExp(int level) //���緹���� �ִ����ġ ���
    {
        if (level >= 1 && level <= 20)
        {
            requireExp = 5 + (level - 1) * 10;
        }
        else if (level >= 21 && level <= 40)
        {
            requireExp = 5 + 10 * 20 + (level - 20) * 13;
        }
        else
        {
            requireExp = 5 + 10 * 20 + 13 * 20 + (level - 40) * 16;
        }
        return requireExp;
    }


    public static GameManager Instance //�ν��Ͻ��� �����ϱ� ���� ������Ƽ
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindObjectOfType<GameManager>();
                if (_inst == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GameManager).ToString();
                    _inst = obj.AddComponent<GameManager>();
                }
            }
            return _inst;
        }
    }

    private void Initialize()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Awake()
    {
        Initialize();

        LoadStatusData();
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private bool timerStart = false;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            if (curGold > 0)
            {
                SaveData.MyGold += curGold;
                curGold = SaveData.MyGold;
            }
            else
            {
                curGold = SaveData.MyGold;
            }

        }

        if (scene.buildIndex == 1)
        {
            WoodChangeAct = null;
            StoneChangeAct = null;
            IronChangeAct = null;
            GoldChangeAct = null;
            ExpChangeAct = null;
            ChangeDayAct = null;
            ChangeAMPMAct = null;
        }

        if (scene.buildIndex == 2)
        {
            SaveData.MyGold = curGold;
            curGold = 0;

            gameStart = true;
            gameOver = false;
            curSec = 0;
            curMin = 0;
            curTime = 0;
            checkAMPMTime = 0;
            isNight = false;
            curDay = 0;
            myWood = 0;
            myStone = 0;
            myIron = 0;
            curLvExp = 0;
            overExp = 0;
            myLevel = 1;

            timerStart = true;
            Sunlight = GameObject.Find("Directional Light").GetComponent<Light>();
        }
    }

    void Start()
    {
        gameStart = true;
        //myWoodCount.text = $"wood : {myWood.ToString()}";
        CalcRequireExp(myLevel);



    }
    /*
    private void UpdateWoodText()
    {
        playerUI.ChangeWoodText(myWood);
    }
    private void UpdateStoneText()
    {
        playerUI.ChangeStoneText(myStone);
    }

    private void UpdateIronText()
    {
        playerUI.ChangeIronText(myIron);
    }
    private void UpdateExpText()
    {
        playerUI.ChangeExpText(myExp);
    }

    private void UpdateExpBar()
    {
        playerUI.ChangeExpBar(myExp);
    }
    
    */

    void Update()
    {

        if (!timerStart)
            return;
                    
        if (gameStart && !gamePaused && !gameOver && curSecText!=null)
        {
            curSec += Time.deltaTime;
            checkAMPMTime += Time.deltaTime;
            TimeConvert();
            //Debug.Log(curGameTime);

            //�㳷 ����
            if (!isNight && checkAMPMTime >= 7)
            {
                checkAMPMTime = 0;
                ChangeToNight();
            }
            if (isNight && checkAMPMTime >= 7)
            {
                checkAMPMTime = 0;
                ChangeToDay();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (!gamePaused)
            {
                PauseGame();
                return;
            }
            if (gamePaused)
            {
                ResumeGame();
                return;
            }
        }


    }

    public void TimeConvert()//�ð��� ��:�� �� ǥ�����ִ� �Լ�
    {
        if(curMin < 10)
        {
            curMinText.text = $"0{Mathf.FloorToInt(curSec / 60).ToString()}";
        }
        else
        {
            curMinText.text = $"{Mathf.FloorToInt(curSec / 60).ToString()}";
        }
        if(curSec<10)
        {
            curSecText.text = $":0{Mathf.FloorToInt(curSec % 60).ToString()}";
        }
        else
        {
            curSecText.text = $":{Mathf.FloorToInt(curSec % 60).ToString()}";
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        Player.SetInputState(false);
        Debug.Log("�Ͻ������Ǿ����ϴ�.");
        //UI �˾� �߰�
    }
    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        Player.SetInputState(true);
        Debug.Log("�Ͻ���������.");
        //UI ������� �߰�
    }
    public void GameOver() // �÷��̾ ��������� ���� �߰��ؾߵ�.
    {
        gameOver = true;
        timerStart = false;
        Time.timeScale = 0;
        StopAllCoroutines();
        //여기에 게임오버 창 뜨게 설정
        UIManager.Instance.CreateUI(UIID.GameOverUI, CanvasType.DynamicCanvas);
    }


    public float CurTime() // ���� ���� �ð�
    {
        return curSec;
    }

    public int CurWood()
    {
        return myWood;
    }

    public int CurStone()
    {
        return myStone;
    }
    public int CurIron() 
    {
        return myIron;
    }
    public int CurGold()
    {
        //Debug.Log(myGold);
        return curGold;
    }
    public int CurExp()
    {
        return curLvExp;
    }


    void ChangeToDay()
    {
        isNight = false;
        Debug.Log("���� �Ǿ����ϴ�.");
        ChangeAMPMAct?.Invoke(isNight);
        StartCoroutine(ChangeSunlightToDay(Sunlight));
        //Sunlight.intensity = 1;
        curDay += 1;
        ChangeDayAct?.Invoke(curDay);
    }
    public IEnumerator ChangeSunlightToDay(Light light)
    {
        float startIntensity = light.intensity; // ���� ����
        float startTime = Time.time; // ���� �ð�

        while (Time.time - startTime < 5f)
        {
            float t = (Time.time - startTime) / 5f; // ���� ����� ���
            light.intensity = Mathf.Lerp(startIntensity, 1f, t); // ���� �������� ��ǥ �������� ������ ����
            yield return null;
        }
        light.intensity = 1f; 
    }
    void ChangeToNight()
    {
        isNight = true;
        Debug.Log("���� �Ǿ����ϴ�.");
        ChangeAMPMAct?.Invoke(isNight);
        StartCoroutine(ChangeSunlightToNight(Sunlight));   
    }
    public IEnumerator ChangeSunlightToNight(Light light)
    {
        float startIntensity = light.intensity; // ���� ����
        float startTime = Time.time; // ���� �ð�

        while (Time.time - startTime < 5f)
        {
            float t = (Time.time - startTime) / 5f; // ���� ����� ���
            light.intensity = Mathf.Lerp(startIntensity, 0f, t); // ���� �������� ��ǥ �������� ������ ����
            yield return null;
        }
        light.intensity = 0f; 
    }

    public void ChangeWood(int num)
    {
        myWood += num;
        //myWoodCount.text = myWood.ToString();
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        WoodChangeAct?.Invoke(myWood);
        Debug.Log($"���� ���� ���� {myWood}");
    }
    public void ChangeStone(int num)
    {
        myStone += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        StoneChangeAct?.Invoke(myStone);
        Debug.Log($"�� ���� ���� {myStone}");
    }
    public void ChangeIron(int num)
    {
        myIron += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        IronChangeAct?.Invoke(myIron);
        Debug.Log($"ö ���� ����. ���� ö : {myIron}");
    }

    public void ChangeGold(int num)
    {
        curGold += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        GoldChangeAct?.Invoke(curGold);
        Debug.Log($"��� ����. ���� ��� : {curGold}");
    }
    
    public void ChangeExp(int num)
    {
        curLvExp += num;
        if (curLvExp >= requireExp) //������
        {
            LevelUp();
        }
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        ExpChangeAct?.Invoke(curLvExp);
    }

    public void LevelUp()
    {
        overExp = curLvExp - requireExp;// ��ģ ����ġ �ӽ�����
        curLvExp = 0;
        myLevel++;
        BlessManager.Instance.RerollCount = SaveData.RerollCount;
        BlessManager.Instance.AppearRandomBlessList();
        requireExp = CalcRequireExp(myLevel);//�������� �䱸 ����ġ�� ���
        curLvExp += overExp;


    }

    public SaveDatas SaveData => _saveData;
    [SerializeField]private SaveDatas _saveData = new SaveDatas();

    [System.Serializable]
    public class SaveDatas
    {
        public int Attack;
        public int AkSp;
        public int MvSp;
        public int MagnetFieldRange;
        public int MaxHp;
        public int ExpBonus;

        public int MyGold;
        public int RerollCount;
    }

    private void OnApplicationQuit()
    {
        SaveStatusData();
    }

    private void SaveStatusData()
    {
        string json = JsonUtility.ToJson(_saveData);
        File.WriteAllText(Application.persistentDataPath + FilePath.StatusData, json);
    }

    private void LoadStatusData()
    {
        string path = Application.persistentDataPath + FilePath.StatusData;
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _saveData = JsonUtility.FromJson<SaveDatas>(json);
        }
        //curGold = _saveData.MyGold;
        //myGold = _saveData.MyGold;
    }
}


