using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

public class EffectPoolManager : Singleton<EffectPoolManager> // �̱��� ����. ����Ʈ ������Ʈ Ǯ�� �Ŵ���.
{
    [SerializeField]public Dictionary<string, Stack<GameObject>> myPool = new Dictionary<string, Stack<GameObject>>();
    /*private void Awake()
    {
        base.Initialize(this);
    }*/
    
    public GameObject SetActiveEffect<T>(GameObject org, GameObject pos) //����Ʈ Ǯ��, ����. (������ �ƴ� ����Ʈ��)
    {
        string Key = typeof(T).ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        if (myPool.ContainsKey(Key)) //�̹� ������ stack�� �ִ� ���
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                obj.transform.position = new Vector3(pos.transform.position.x , 1 , pos.transform.position.z);
                obj.SetActive(true); //Ȱ��ȭ
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        GameObject Effect = Instantiate(org, new Vector3(pos.transform.position.x, 1, pos.transform.position.z), Quaternion.identity); //���°�� ���� ����
        return Effect;
    }
    

    //EffectPoolManager.instance.GetObject<Ŭ������>(GameObject org, Transform p); �������� �����ϸ� �ȴ�.
    public GameObject SetActiveRangeObject<T>(GameObject org, Transform parent, GameObject pos,  float atk = 1, float radius = 1, 
        float size = 1, float speed = 1, float atkSpeed = 1, float hitDelay = 1 ,float durTime = 1) // ������Ʈ Ȱ��ȭ, ����
    {
        string Key = typeof(T).ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        if (myPool.ContainsKey(Key)) //�̹� ������ stack�� �ִ� ���
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop(); 
                                                    //obj.transform.SetParent(parent); //�θ� ����
                ISetPointStats reSetStats = obj.GetComponent<ISetPointStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetPointStats(atk, radius, size, speed, atkSpeed ,hitDelay,durTime);
                }
                obj.transform.position = new Vector3(pos.transform.position.x,obj.transform.position.y,pos.transform.position.z);
                obj.SetActive(true); //Ȱ��ȭ
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//���°�� �����Ѵ�.

        GameObject atkEffect = Instantiate(org, transform.position, org.transform.rotation); //���°�� ���� ����
        ISetPointStats setStats = atkEffect.GetComponent<ISetPointStats>(); //���� ������Ʈ�� ��� ISetStats�� ���� �־�� �Ѵ�.
        if (setStats != null)
        {
            setStats.SetPointStats(atk, radius, size, speed, atkSpeed,hitDelay, durTime); // ���� ����.
        }
        atkEffect.transform.position = new Vector3(pos.transform.position.x, org.transform.position.y, pos.transform.position.z); // �Ű������� �Ѱܹ��� ������ġ�� �̵�.

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//���°�� �����Ѵ�.

    }

    public GameObject SetActiveProjectileObject<T>(GameObject org, Transform parent, GameObject pos, float atk = 1, float size = 1, float speed = 1,
        float range = 1,bool canPenetrate = false, int penetrateCount = 0, Vector3 dir = default(Vector3)) // ������Ʈ Ȱ��ȭ, ����
    {
        string Key = typeof(T).ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        if (myPool.ContainsKey(Key)) //�̹� ������ stack�� �ִ� ���
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                //obj.transform.SetParent(parent); //�θ� ����
                ISetProjectileStats reSetStats = obj.GetComponent<ISetProjectileStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetProjectileStats(atk, size, speed,range, canPenetrate, penetrateCount);
                }
                obj.transform.position = pos.transform.position;
                obj.SetActive(true); //Ȱ��ȭ
                obj.transform.rotation = Quaternion.LookRotation(dir);
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//���°�� �����Ѵ�.

        GameObject atkEffect = Instantiate(org, transform.position, org.transform.rotation); //���°�� ���� ����
        ISetProjectileStats setStats = atkEffect.GetComponent<ISetProjectileStats>(); //���� ������Ʈ�� ��� ISetStats�� ���� �־�� �Ѵ�.
        setStats.SetProjectileStats(atk, size, speed, range, canPenetrate, penetrateCount); // ���� ����.
        atkEffect.transform.position = pos.transform.position; // �Ű������� �Ѱܹ��� ������ġ�� �̵�.
        atkEffect.SetActive(true);

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//���°�� �����Ѵ�.

    }

    /*
    //������ ���� ���� ������Ʈ ����. (ex ���ðǹ�) �ڽ����� ������.
    public GameObject SetActiveMeeleLastObject<T>(GameObject org, Transform parent, GameObject pos, short atk = 1, float radius = 1, float size = 1, float speed = 1,
        float atkDelay = 1) // ������Ʈ Ȱ��ȭ, ����
    {
        string Key = typeof(T).ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        if (myPool.ContainsKey(Key)) //�̹� ������ stack�� �ִ� ���
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                obj.transform.SetParent(parent); //�θ� ����
                ISetPointStats reSetStats = obj.GetComponent<ISetPointStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetPointStats(atk, radius, size, speed, atkDelay);
                }
                obj.transform.position = pos.transform.position + new Vector3 (0,0.2f,0);
                obj.SetActive(true); //Ȱ��ȭ
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//���°�� �����Ѵ�.

        GameObject atkEffect = Instantiate(org, transform.position, Quaternion.identity); //���°�� ���� ����
        ISetPointStats setStats = atkEffect.GetComponent<ISetPointStats>();
        setStats.SetPointStats(atk, radius, size, speed); // ���� ����.
        atkEffect.transform.position = pos.transform.position + new Vector3(0, 0.2f, 0); // �Ű������� �Ѱܹ��� ������ġ�� �̵�. + ���� ����Ʈ�� �������� �ʰ� y�� 0.2+

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//���°�� �����Ѵ�.

    }
    */



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