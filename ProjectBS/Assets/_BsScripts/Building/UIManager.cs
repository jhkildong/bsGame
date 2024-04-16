using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public GameObject BuildPopUp;
    private bool BuildIsOpen = false;

    public static UIManager Instance //�ν��Ͻ��� �����ϱ� ���� ������Ƽ
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;

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
