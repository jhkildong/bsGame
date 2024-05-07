using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPoolManager : Singleton<EffectPoolManager> // �̱��� ����. ����Ʈ ������Ʈ Ǯ�� �Ŵ���.
{
    [SerializeField]public Dictionary<string, Stack<GameObject>> myPool = new Dictionary<string, Stack<GameObject>>();
    /*private void Awake()
    {
        base.Initialize(this);
    }*/

    public GameObject SetActiveHitEffect(HitEffects org, Vector3 pos, int key, Transform parent = null)
    {
        Vector3 instPos = new Vector3(pos.x, 1f, pos.z);
        string Key = key.ToString();

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0)
        {
            // Ǯ�� �ش��ϴ� Ű�� �ְ�, Ǯ�� ������Ʈ�� �ִ� ���
            GameObject obj = myPool[Key].Pop();
            obj.transform.position = instPos;
            obj.SetActive(true);
        }
        else
        {
            // Ǯ�� �ش��ϴ� Ű�� ���ų� Ǯ�� ������Ʈ�� ���� ���
            GameObject effect = Instantiate(org.gameObject, instPos, Quaternion.identity);
            if (!myPool.ContainsKey(Key))
            {
                myPool[Key] = new Stack<GameObject>();
            }
            myPool[Key].Push(effect);
        }
        return null;
    }
    public GameObject SetParentEffect(HitEffects org, int key, Transform parent = null) //�θ������Ʈ�� �ڽĿ� �����Ǿ��ϴ� ����Ʈ��
    {
        string Key = key.ToString();

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0)
        {
            // Ǯ�� �ش��ϴ� Ű�� �ְ�, Ǯ�� ������Ʈ�� �ִ� ���
            GameObject obj = myPool[Key].Pop();
            obj.transform.position = parent.transform.position;   
            obj.transform.SetParent(parent);
            obj.SetActive(true);
        }
        else
        {
            // Ǯ�� �ش��ϴ� Ű�� ���ų� Ǯ�� ������Ʈ�� ���� ���
            GameObject effect = Instantiate(org.gameObject, parent.transform.position, Quaternion.identity);
            effect.transform.SetParent(parent);
            if (!myPool.ContainsKey(Key))
            {
                myPool[Key] = new Stack<GameObject>();
            }
            myPool[Key].Push(effect);
        }
        return null;
    }


    public GameObject SetActiveEffect<T>(GameObject org, GameObject pos, Transform parent = null) //����Ʈ Ǯ��, ����. (������ �ƴ� ����Ʈ��)
    {
        Vector3 instPos = new Vector3(pos.transform.position.x, 1f, pos.transform.position.z);
        string Key = typeof(T).ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        if (myPool.ContainsKey(Key)) //�̹� ������ stack�� �ִ� ���
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                obj.transform.position = instPos;
                obj.SetActive(true); //Ȱ��ȭ
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        GameObject Effect = Instantiate(org, instPos, Quaternion.identity); //���°�� ���� ����
        return Effect;
    }

    public GameObject SetActiveRangeObject(PointAtkEffectHit org, Transform parent, GameObject pos, int key = 1, float atk = 1, float radius = 1,
    float size = 1, float speed = 1, float atkSpeed = 1, float hitDelay = 1, float durTime = 1) // ������Ʈ Ȱ��ȭ, ����
    {
        string Key = key.ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        GameObject atkEffect;

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0) //�̹� ������ stack�� �ִ� ���
        {

                atkEffect = myPool[Key].Pop();
                //obj.transform.SetParent(parent); //�θ� ����
                ISetPointStats reSetStats = atkEffect.GetComponent<ISetPointStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetPointStats(atk, radius, size, speed, atkSpeed, hitDelay, durTime);
                }
                atkEffect.transform.position = new Vector3(pos.transform.position.x, atkEffect.transform.position.y, pos.transform.position.z);
                atkEffect.SetActive(true); //Ȱ��ȭ
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;
            
        }

        //return Instantiate(org, p);//���°�� �����Ѵ�.
        else
        {
            atkEffect = Instantiate(org.gameObject, transform.position, org.transform.rotation); //���°�� ���� ����
            atkEffect.SetActive (false);
            ISetPointStats setStats = atkEffect.GetComponent<ISetPointStats>(); //���� ������Ʈ�� ��� ISetStats�� ���� �־�� �Ѵ�.
            if (setStats != null)
            {
                setStats.SetPointStats(atk, radius, size, speed, atkSpeed, hitDelay, durTime); // ���� ����.
            }

            atkEffect.transform.position = new Vector3(pos.transform.position.x, org.transform.position.y, pos.transform.position.z); // �Ű������� �Ѱܹ��� ������ġ�� �̵�.
            atkEffect.SetActive(true);
            /*
            if (!myPool.ContainsKey(Key))
            {
                myPool[Key] = new Stack<GameObject>();
            }
            myPool[Key].Push(atkEffect);
        */
        }
        return null;
    }
    /*
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
    }*/

    public GameObject SetActiveProjectileObject(ProjectileEffectHit org, Transform parent, GameObject pos, int key ,float atk = 1, float size = 1, float speed = 1,
        float range = 1, bool canPenetrate = false, int penetrateCount = 0, Vector3 dir = default(Vector3)) // ������Ʈ Ȱ��ȭ, ����
    {
        string Key = key.ToString(); //T�� Ŭ���� ���� �ɰ�. Ŭ�������� key������ ����ϰڴٴ� �ǹ�.
        GameObject atkEffect;

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0) //�̹� ������ stack�� �ִ� ���
        {

                atkEffect = myPool[Key].Pop();
                //obj.transform.SetParent(parent); //�θ� ����
                ISetProjectileStats reSetStats = atkEffect.GetComponent<ISetProjectileStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetProjectileStats(atk, size, speed, range, canPenetrate, penetrateCount);
                }
                atkEffect.transform.position = pos.transform.position;
                atkEffect.SetActive(true); //Ȱ��ȭ
                atkEffect.transform.rotation = Quaternion.LookRotation(dir);
                //obj.transform.localPosition = p.localPosition; //�ش���ġ�� �̵�
                //obj.transform.localRotation = p.rotation;

        }

        //return Instantiate(org, p);//���°�� �����Ѵ�.
        else
        {
            atkEffect = Instantiate(org.gameObject, transform.position, org.transform.rotation); //���°�� ���� ����
            ISetProjectileStats setStats = atkEffect.GetComponent<ISetProjectileStats>(); //���� ������Ʈ�� ��� ISetStats�� ���� �־�� �Ѵ�.
            atkEffect.SetActive(false);
            if (setStats != null)
            {
                setStats.SetProjectileStats(atk, size, speed, range, canPenetrate, penetrateCount);
            }

            atkEffect.transform.position = pos.transform.position; // �Ű������� �Ѱܹ��� ������ġ�� �̵�.
            atkEffect.SetActive(true);
            atkEffect.transform.rotation = Quaternion.LookRotation(dir);
            if (!myPool.ContainsKey(Key))
            {
                myPool[Key] = new Stack<GameObject>();
            }
            myPool[Key].Push(atkEffect);
        }
        return null;
    }


    public void ReleaseObject(GameObject obj , int key) // ������Ʈ�� Ǯ�� �ǵ�����.
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        string Key = key.ToString();
        if (!myPool.ContainsKey(Key))
        {
            myPool[Key] = new Stack<GameObject>();
        }
        myPool[Key].Push(obj);
    }

    /*    public void ReleaseObject<T>(GameObject obj) // ������Ʈ�� Ǯ�� �ǵ�����.
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
    */


}