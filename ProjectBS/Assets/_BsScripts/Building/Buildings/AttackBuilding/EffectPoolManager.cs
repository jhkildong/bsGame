using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPoolManager : Singleton<EffectPoolManager> // �̱��� ����. ����Ʈ ������Ʈ Ǯ�� �Ŵ���.
{
    [SerializeField]public Dictionary<string, Stack<GameObject>> myPool = new Dictionary<string, Stack<GameObject>>();
    private void Awake()
    {
        base.Initialize();
    }

    //EffectPoolManager.instance.GetObject<Ŭ������>(GameObject org, Transform p); �������� �����ϸ� �ȴ�.
    public GameObject SetActiveObject<T>(GameObject org, Transform parent, GameObject pos,  short atk = 1, float radius = 1, float size = 1, float speed = 1) // ������Ʈ Ȱ��ȭ, ����
    {
        string Key = typeof(T).ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        if (myPool.ContainsKey(Key)) //�̹� ������ stack�� �ִ� ���
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop(); //�����ֱٿ� ���°� return���ش�.
                                                    //obj.transform.SetParent(parent); //�θ� ����
                ISetStats reSetStats = obj.GetComponent<ISetStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetStats(atk, radius, size, speed);
                }
                obj.transform.position = pos.transform.position;
                obj.SetActive(true); //Ȱ��ȭ
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//���°�� �����Ѵ�.

        GameObject atkEffect = Instantiate(org, transform.position, Quaternion.identity); //���°�� ���� ����
        ISetStats setStats = atkEffect.GetComponent<ISetStats>(); //���� ������Ʈ�� ��� ISetStats�� ���� �־�� �Ѵ�.
        setStats.SetStats(atk, radius, size, speed); // ���� ����.
        atkEffect.transform.position = pos.transform.position; // �Ű������� �Ѱܹ��� ������ġ�� �̵�.

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//���°�� �����Ѵ�.

    }

    public void ReleaseObject<T>(GameObject obj) // ������Ʈ�� Ǯ�� �ǵ�����.
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        string Key = typeof(T).ToString();
        if (!myPool.ContainsKey(Key))
        {
            myPool[Key] = new Stack<GameObject>();
        }
        myPool[Key].Push(obj);
    }


}