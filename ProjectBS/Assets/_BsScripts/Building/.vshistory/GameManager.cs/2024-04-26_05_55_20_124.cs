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

    public TextMeshProUGUI myWoodCount;
    public TextMeshProUGUI myStoneCount;
    public TextMeshProUGUI myIronCount;

    public Player Player;

    public bool gameStart;
    public bool gameOver;
    public bool gamePaused;
    public float curGameTime; // ����ð�
    public float checkAMPMTime;
    private bool isNight;
    //private int myExp;
    private int myWood;
    private int myStone;
    private int myIron;
    private int myGold;

    private int myLevel;
    private int myExp;
    private int MaxExp;

    public event UnityAction<int> WoodChangeAct;
    public event UnityAction<int> StoneChangeAct;
    public event UnityAction<int> IronChangeAct;
    public event UnityAction<int> GoldChangeAct;
    public event UnityAction<int> ExpChangeAct;

    private PlayerUI playerUI;


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


        //�ڿ������ �Ͼ ��� �̺�Ʈ ���
        /*
            WoodChangeAct += UpdateWoodText;
            StoneChangeAct += UpdateStoneText;
            IronChangeAct += UpdateIronText;
            ExpChangeAct += UpdateExpText;
            //ExpChangeAct += UpdateExpText;

        */

    }
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

    /*
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
        return myExp;
    }


    void ChangeToDay()
    {
        isNight = false;
        Debug.Log("���� �Ǿ����ϴ�.");
    }
    void ChangeToNight()
    {
        isNight = true;
        Debug.Log("���� �Ǿ����ϴ�.");
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
        myExp += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        ExpChangeAct?.Invoke(myExp);
        Debug.Log($"����ġ ����. ���� ����ġ : {myExp}");
    }
}


