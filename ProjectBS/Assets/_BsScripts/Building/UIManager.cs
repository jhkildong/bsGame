using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public GameObject BuildPopUp;
    private bool BuildIsOpen = false;

    public static UIManager Instance //인스턴스에 접근하기 위한 프로퍼티
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;

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
        BuildPopUp.SetActive(false);
        BuildIsOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!BuildIsOpen)
            {
                OpenBuildTab();
            }
            else
            {
                CloseBuildTab();
            }
        }    
    }

    public void OpenBuildTab()
    {
        if(!BuildIsOpen)
        {
            BuildPopUp.SetActive(true);
            BuildIsOpen = true;
        }
    }
    public void CloseBuildTab()
    {
        if (BuildIsOpen)
        {
            BuildPopUp.SetActive(false);
            BuildIsOpen = false;
        }
    }
}
