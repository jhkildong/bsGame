using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������� ����� ��Ŭ�� Ŭ����. �̱����� ����� ������Ʈ�� ��ӹ޾� ����ϸ� �ȴ�.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // where -> T�� monobehavior�� ��ӹޱ� ������ �������̴�. ( ��ȣ���� ���ֱ� ���� ��������ڸ� ����ߴ�.)
{
    //�̱����� ��𼭵� �ս��� ���ٰ���
    private static T _inst;
    public static T Instance
    {
        get
        {
            if (_inst == null) // ó�� ȣ��ȴٸ�
            {
                _inst = FindObjectOfType<T>(); //���ϼ� ������ ����
                if (_inst == null)//ã�����ߴٸ� -> �������� �ʴ´ٸ�
                {
                    GameObject obj = new GameObject(); //���ӿ�����Ʈ�� �����
                    obj.name = typeof(T).ToString(); // ���� ������Ʈ�� �̸��� T(class�̸�)���� �ٲ��ش�. ( �˱� ���� )
                    _inst = obj.AddComponent<T>(); // ���ӿ�����Ʈ�� �� ������Ʈ�� �߰����ش�.
                }
            }
            return _inst;
        }

    }
    protected void Initialize(T This)
    {
        if(_inst == null)
            _inst = This; // �ν��Ͻ��� ������ �ڱ��ڽ��� �������� �����ڿ� �����϶�� ���ش�.
        if (_inst != null && _inst != this) //�̹� ������ �ν��Ͻ��� �ִٸ�
        {
            Destroy(this.gameObject); //�̹��� ������ �ڽ��� �ı��Ͽ� ���ϼ��� ����
        }
        else
        {
            DontDestroyOnLoad(gameObject); // ���� �̵��ص� �������� �ʰ�.
        }
    }
    protected virtual void Awake()
    {
        Initialize(this as T);
    }
}
