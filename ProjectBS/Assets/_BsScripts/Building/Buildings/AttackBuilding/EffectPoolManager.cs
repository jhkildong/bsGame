using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

public class EffectPoolManager : Singleton<EffectPoolManager> // 싱글턴 패턴. 이펙트 오브젝트 풀링 매니저.
{
    [SerializeField]public Dictionary<string, Stack<GameObject>> myPool = new Dictionary<string, Stack<GameObject>>();
    /*private void Awake()
    {
        base.Initialize(this);
    }*/
    
    public GameObject SetActiveEffect<T>(GameObject org, GameObject pos) //이펙트 풀링, 생성. (공격이 아닌 이펙트들)
    {
        string Key = typeof(T).ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        if (myPool.ContainsKey(Key)) //이미 생성된 stack이 있는 경우
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                obj.transform.position = new Vector3(pos.transform.position.x , 1 , pos.transform.position.z);
                obj.SetActive(true); //활성화
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        GameObject Effect = Instantiate(org, new Vector3(pos.transform.position.x, 1, pos.transform.position.z), Quaternion.identity); //없는경우 새로 생성
        return Effect;
    }
    

    //EffectPoolManager.instance.GetObject<클래스명>(GameObject org, Transform p); 형식으로 접근하면 된다.
    public GameObject SetActiveRangeObject<T>(GameObject org, Transform parent, GameObject pos,  float atk = 1, float radius = 1, 
        float size = 1, float speed = 1, float atkSpeed = 1, float hitDelay = 1 ,float durTime = 1) // 오브젝트 활성화, 생성
    {
        string Key = typeof(T).ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        if (myPool.ContainsKey(Key)) //이미 생성된 stack이 있는 경우
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop(); 
                                                    //obj.transform.SetParent(parent); //부모 설정
                ISetPointStats reSetStats = obj.GetComponent<ISetPointStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetPointStats(atk, radius, size, speed, atkSpeed ,hitDelay,durTime);
                }
                obj.transform.position = new Vector3(pos.transform.position.x,obj.transform.position.y,pos.transform.position.z);
                obj.SetActive(true); //활성화
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//없는경우 생성한다.

        GameObject atkEffect = Instantiate(org, transform.position, org.transform.rotation); //없는경우 새로 생성
        ISetPointStats setStats = atkEffect.GetComponent<ISetPointStats>(); //공격 오브젝트는 모두 ISetStats를 갖고 있어야 한다.
        if (setStats != null)
        {
            setStats.SetPointStats(atk, radius, size, speed, atkSpeed,hitDelay, durTime); // 스탯 설정.
        }
        atkEffect.transform.position = new Vector3(pos.transform.position.x, org.transform.position.y, pos.transform.position.z); // 매개변수로 넘겨받은 공격위치로 이동.

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//없는경우 생성한다.

    }

    public GameObject SetActiveProjectileObject<T>(GameObject org, Transform parent, GameObject pos, float atk = 1, float size = 1, float speed = 1,
        float range = 1,bool canPenetrate = false, int penetrateCount = 0, Vector3 dir = default(Vector3)) // 오브젝트 활성화, 생성
    {
        string Key = typeof(T).ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        if (myPool.ContainsKey(Key)) //이미 생성된 stack이 있는 경우
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                //obj.transform.SetParent(parent); //부모 설정
                ISetProjectileStats reSetStats = obj.GetComponent<ISetProjectileStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetProjectileStats(atk, size, speed,range, canPenetrate, penetrateCount);
                }
                obj.transform.position = pos.transform.position;
                obj.SetActive(true); //활성화
                obj.transform.rotation = Quaternion.LookRotation(dir);
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//없는경우 생성한다.

        GameObject atkEffect = Instantiate(org, transform.position, org.transform.rotation); //없는경우 새로 생성
        ISetProjectileStats setStats = atkEffect.GetComponent<ISetProjectileStats>(); //공격 오브젝트는 모두 ISetStats를 갖고 있어야 한다.
        setStats.SetProjectileStats(atk, size, speed, range, canPenetrate, penetrateCount); // 스탯 설정.
        atkEffect.transform.position = pos.transform.position; // 매개변수로 넘겨받은 공격위치로 이동.
        atkEffect.SetActive(true);

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//없는경우 생성한다.

    }

    /*
    //지속형 근접 공격 오브젝트 생성. (ex 가시건물) 자식으로 생성됨.
    public GameObject SetActiveMeeleLastObject<T>(GameObject org, Transform parent, GameObject pos, short atk = 1, float radius = 1, float size = 1, float speed = 1,
        float atkDelay = 1) // 오브젝트 활성화, 생성
    {
        string Key = typeof(T).ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        if (myPool.ContainsKey(Key)) //이미 생성된 stack이 있는 경우
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                obj.transform.SetParent(parent); //부모 설정
                ISetPointStats reSetStats = obj.GetComponent<ISetPointStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetPointStats(atk, radius, size, speed, atkDelay);
                }
                obj.transform.position = pos.transform.position + new Vector3 (0,0.2f,0);
                obj.SetActive(true); //활성화
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        //return Instantiate(org, p);//없는경우 생성한다.

        GameObject atkEffect = Instantiate(org, transform.position, Quaternion.identity); //없는경우 새로 생성
        ISetPointStats setStats = atkEffect.GetComponent<ISetPointStats>();
        setStats.SetPointStats(atk, radius, size, speed); // 스탯 설정.
        atkEffect.transform.position = pos.transform.position + new Vector3(0, 0.2f, 0); // 매개변수로 넘겨받은 공격위치로 이동. + 땅에 이펙트가 가려지지 않게 y로 0.2+

        return atkEffect;
        //return Instantiate(org, pos.transform.position,Quaternion.identity, parent);//없는경우 생성한다.

    }
    */



    public void ReleaseObject<T>(GameObject obj) // 오브젝트를 풀로 되돌린다.
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