using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������� ����� ��Ŭ�� Ŭ����. �̱����� ����� ������Ʈ�� ��ӹ޾� ����ϸ� �ȴ�.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // where -> T�� monobehavior�� ��ӹޱ� ������ �������̴�. ( ��ȣ���� ���ֱ� ���� ��������ڸ� ����ߴ�.)
{

    //�̱����� ��𼭵� �ս��� ���ٰ���
    static T _inst = null;
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
                    obj.name = typeof(T).ToString(); // ���� ������Ʈ�� �̸��� Singleton���� �ٲ��ش�. ( �˱� ���� )
                    _inst = obj.AddComponent<T>(); // ���ӿ�����Ʈ�� �� ������Ʈ�� �߰����ش�.
                }
            }
            return _inst;
        }

    }
    protected void Initialize()
    {
        //_inst = this; // �������� �ڱ��ڽ��� �����ڿ� �����϶�� ���ش�.
        if (_inst != null && _inst != this) //�ٸ� �ν��Ͻ��� �����ɶ�
        {
            Destroy(this.gameObject); //�̹��� ������ �ڽ��� �ı��Ͽ� ���ϼ��� ����
        }
        else
        {
            DontDestroyOnLoad(gameObject); // ���� �̵��ص� �������� �ʰ�.
        }
    }
}
