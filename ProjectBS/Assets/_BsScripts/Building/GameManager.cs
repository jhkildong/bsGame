using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 패턴을 사용하기 위한 인스턴스 변수. 이렇게 설계하면 객체를 생성하지 않아도 static이기 때문에 외부 어디서든 접근 가능

    //GameManager.Instance.StartGame(); 외부에서 이런 식으로 접근 가능하다.

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



    public static GameManager Instance //인스턴스에 접근하기 위한 프로퍼티
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 인스턴스가 없습니다");
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다. (GameManager 인스턴스는 언제나 하나여야 한다 )
        else if (_instance != this)
        {
            Debug.Log("씬에 두개이상의 인스턴스가 있습니다");
            Destroy(gameObject);
        }
        //이렇게 하면 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
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

            //밤낮 변경
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
        Debug.Log("일시정지되었습니다.");
        //UI 팝업 추가
    }
    void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        Debug.Log("일시정지해제.");
        //UI 사라지게 추가
    }
    public void GameOver() // 플레이어가 사망했을때 조건 추가해야됨.
    {
        gameOver = true;
        Debug.Log("게임오버");
    }

    void ChangeToDay()
    {
        isNight = false;
        Debug.Log("낮이 되었습니다.");
    }
    void ChangeToNight()
    {
        isNight = true;
        Debug.Log("밤이 되었습니다.");
    }

    void ChangeWood(int num)
    {
        myWood += num;
        //UI로 나타낼 코드 추가 필요
        Debug.Log($"나무 갯수 변동 {myWood}");
    }
    void ChangeStone(int num)
    {
        myStone += num;
        //UI로 나타낼 코드 추가 필요
        Debug.Log($"돌 갯수 변동 {myStone}");
    }
    void ChangeIron(int num)
    {
        myIron += num;
        //UI로 나타낼 코드 추가 필요
        Debug.Log($"철 갯수 변동 {myIron}");
    }

    void ChangeGold(int num)
    {
        myIron += num;
        //UI로 나타낼 코드 추가 필요
        Debug.Log($"철 갯수 변동 {myGold}");
    }

}


