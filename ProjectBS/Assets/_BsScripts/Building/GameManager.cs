using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //�̱��� ������ ����ϱ� ���� �ν��Ͻ� ����. �̷��� �����ϸ� ��ü�� �������� �ʾƵ� static�̱� ������ �ܺ� ��𼭵� ���� ����

    //GameManager.Instance.StartGame(); �ܺο��� �̷� ������ ���� �����ϴ�.

    private static GameManager _instance;


    public Player Player;

    public bool gameStart;
    public bool gameOver;
    public bool gamePaused;
    public float curGameTime; // ����ð�
    public float checkAMPMTime;
    private bool isNight;
    public int curDay;
    //private int myExp;
    private int myWood = 999;
    private int myStone = 999;
    private int myIron = 999;
    private int myGold = 999;

    private int myLevel = 1; // �� ����
    private int myExp; //����ġ �ѷ�
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
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                {
                    Debug.Log("�̱��� �ν��Ͻ��� �����ϴ�");
                }

            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�. (GameManager �ν��Ͻ��� ������ �ϳ����� �Ѵ� )
        else if (_instance != this)
        {
            Debug.Log("���� �ΰ��̻��� �ν��Ͻ��� �ֽ��ϴ�");
            Destroy(gameObject);
        }
        //�̷��� �ϸ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);
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
        if (gameStart && !gamePaused && !gameOver)
        {
            curGameTime += Time.deltaTime;
            checkAMPMTime += Time.deltaTime;
            //Debug.Log(curGameTime);

            //�㳷 ����
            if (!isNight && checkAMPMTime >= 5)
            {
                checkAMPMTime = 0;
                ChangeToNight();
            }
            if (isNight && checkAMPMTime >= 10)
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

    void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        Debug.Log("�Ͻ������Ǿ����ϴ�.");
        //UI �˾� �߰�
    }
    void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        Debug.Log("�Ͻ���������.");
        //UI ������� �߰�
    }
    public void GameOver() // �÷��̾ ��������� ���� �߰��ؾߵ�.
    {
        gameOver = true;
        Debug.Log("���ӿ���");
    }

    public float CurTime() // ���� ���� �ð�
    {
        return curGameTime;
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
        return myGold;
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
        
        curDay += 1;
        ChangeDayAct?.Invoke(curDay);
    }
    void ChangeToNight()
    {
        isNight = true;
        Debug.Log("���� �Ǿ����ϴ�.");
        ChangeAMPMAct?.Invoke(isNight);
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
        myIron += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        GoldChangeAct?.Invoke(myGold);
        Debug.Log($"��� ����. ���� ��� : {myGold}");
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
        BlessManager.Instance.AppearRandomBlessList();
        requireExp = CalcRequireExp(myLevel);//�������� �䱸 ����ġ�� ���
        curLvExp += overExp;


    }

}


