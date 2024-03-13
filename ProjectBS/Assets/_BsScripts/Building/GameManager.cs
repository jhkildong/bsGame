using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�̱��� ������ ����ϱ� ���� �ν��Ͻ� ����. �̷��� �����ϸ� ��ü�� �������� �ʾƵ� static�̱� ������ �ܺ� ��𼭵� ���� ����

    //GameManager.Instance.StartGame(); �ܺο��� �̷� ������ ���� �����ϴ�.

    private static GameManager _instance;

    public bool gameStart;
    public bool gameOver;
    public bool gamePaused;
    private float curGameTime;
    private float checkAMPMTime;
    private bool isNight;
    //private int myExp;
    private int myWood;
    private int myStone;
    private int myIron;
    private int myGold;



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
    }

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
            if (isNight && checkAMPMTime >=10)
            {
                checkAMPMTime = 0;
                ChangeToDay();
            }

            
        }

        if (Input.GetKeyDown(KeyCode.Escape)&& !gameOver)
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

    void ChangeWood(int num)
    {
        myWood += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        Debug.Log($"���� ���� ���� {myWood}");
    }
    void ChangeStone(int num)
    {
        myStone += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        Debug.Log($"�� ���� ���� {myStone}");
    }
    void ChangeIron(int num)
    {
        myIron += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        Debug.Log($"ö ���� ���� {myIron}");
    }

    void ChangeGold(int num)
    {
        myIron += num;
        //UI�� ��Ÿ�� �ڵ� �߰� �ʿ�
        Debug.Log($"ö ���� ���� {myGold}");
    }

}


