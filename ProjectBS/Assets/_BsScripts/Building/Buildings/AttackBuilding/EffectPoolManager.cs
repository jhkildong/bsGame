using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPoolManager : Singleton<EffectPoolManager> // 싱글턴 패턴. 이펙트 오브젝트 풀링 매니저.
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
            // 풀에 해당하는 키가 있고, 풀에 오브젝트가 있는 경우
            GameObject obj = myPool[Key].Pop();
            obj.transform.position = instPos;
            obj.SetActive(true);
        }
        else
        {
            // 풀에 해당하는 키가 없거나 풀에 오브젝트가 없는 경우
            GameObject effect = Instantiate(org.gameObject, instPos, Quaternion.identity);
            if (!myPool.ContainsKey(Key))
            {
                myPool[Key] = new Stack<GameObject>();
            }
            myPool[Key].Push(effect);
        }
        return null;
    }
    public GameObject SetParentEffect(HitEffects org, int key, Transform parent = null) //부모오브젝트의 자식에 생성되야하는 이펙트들
    {
        string Key = key.ToString();

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0)
        {
            // 풀에 해당하는 키가 있고, 풀에 오브젝트가 있는 경우
            GameObject obj = myPool[Key].Pop();
            obj.transform.position = parent.transform.position;   
            obj.transform.SetParent(parent);
            obj.SetActive(true);
        }
        else
        {
            // 풀에 해당하는 키가 없거나 풀에 오브젝트가 없는 경우
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


    public GameObject SetActiveEffect<T>(GameObject org, GameObject pos, Transform parent = null) //이펙트 풀링, 생성. (공격이 아닌 이펙트들)
    {
        Vector3 instPos = new Vector3(pos.transform.position.x, 1f, pos.transform.position.z);
        string Key = typeof(T).ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        if (myPool.ContainsKey(Key)) //이미 생성된 stack이 있는 경우
        {
            if (myPool[Key].Count > 0)
            {
                GameObject obj = myPool[Key].Pop();
                obj.transform.position = instPos;
                obj.SetActive(true); //활성화
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;
                return obj;
            }
        }
        GameObject Effect = Instantiate(org, instPos, Quaternion.identity); //없는경우 새로 생성
        return Effect;
    }

    public GameObject SetActiveRangeObject(PointAtkEffectHit org, Transform parent, GameObject pos, int key = 1, float atk = 1, float radius = 1,
    float size = 1, float speed = 1, float atkSpeed = 1, float hitDelay = 1, float durTime = 1) // 오브젝트 활성화, 생성
    {
        string Key = key.ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        GameObject atkEffect;

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0) //이미 생성된 stack이 있는 경우
        {

                atkEffect = myPool[Key].Pop();
                //obj.transform.SetParent(parent); //부모 설정
                ISetPointStats reSetStats = atkEffect.GetComponent<ISetPointStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetPointStats(atk, radius, size, speed, atkSpeed, hitDelay, durTime);
                }
                atkEffect.transform.position = new Vector3(pos.transform.position.x, atkEffect.transform.position.y, pos.transform.position.z);
                atkEffect.SetActive(true); //활성화
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;
            
        }

        //return Instantiate(org, p);//없는경우 생성한다.
        else
        {
            atkEffect = Instantiate(org.gameObject, transform.position, org.transform.rotation); //없는경우 새로 생성
            atkEffect.SetActive (false);
            ISetPointStats setStats = atkEffect.GetComponent<ISetPointStats>(); //공격 오브젝트는 모두 ISetStats를 갖고 있어야 한다.
            if (setStats != null)
            {
                setStats.SetPointStats(atk, radius, size, speed, atkSpeed, hitDelay, durTime); // 스탯 설정.
            }

            atkEffect.transform.position = new Vector3(pos.transform.position.x, org.transform.position.y, pos.transform.position.z); // 매개변수로 넘겨받은 공격위치로 이동.
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
    }*/

    public GameObject SetActiveProjectileObject(ProjectileEffectHit org, Transform parent, GameObject pos, int key ,float atk = 1, float size = 1, float speed = 1,
        float range = 1, bool canPenetrate = false, int penetrateCount = 0, Vector3 dir = default(Vector3)) // 오브젝트 활성화, 생성
    {
        string Key = key.ToString(); //T는 클래스 명이 될것. 클래스명을 key값으로 사용하겠다는 의미.
        GameObject atkEffect;

        if (myPool.ContainsKey(Key) && myPool[Key].Count > 0) //이미 생성된 stack이 있는 경우
        {

                atkEffect = myPool[Key].Pop();
                //obj.transform.SetParent(parent); //부모 설정
                ISetProjectileStats reSetStats = atkEffect.GetComponent<ISetProjectileStats>();
                if (reSetStats != null)
                {
                    reSetStats.SetProjectileStats(atk, size, speed, range, canPenetrate, penetrateCount);
                }
                atkEffect.transform.position = pos.transform.position;
                atkEffect.SetActive(true); //활성화
                atkEffect.transform.rotation = Quaternion.LookRotation(dir);
                //obj.transform.localPosition = p.localPosition; //해당위치로 이동
                //obj.transform.localRotation = p.rotation;

        }

        //return Instantiate(org, p);//없는경우 생성한다.
        else
        {
            atkEffect = Instantiate(org.gameObject, transform.position, org.transform.rotation); //없는경우 새로 생성
            ISetProjectileStats setStats = atkEffect.GetComponent<ISetProjectileStats>(); //공격 오브젝트는 모두 ISetStats를 갖고 있어야 한다.
            atkEffect.SetActive(false);
            if (setStats != null)
            {
                setStats.SetProjectileStats(atk, size, speed, range, canPenetrate, penetrateCount);
            }

            atkEffect.transform.position = pos.transform.position; // 매개변수로 넘겨받은 공격위치로 이동.
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


    public void ReleaseObject(GameObject obj , int key) // 오브젝트를 풀로 되돌린다.
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

    /*    public void ReleaseObject<T>(GameObject obj) // 오브젝트를 풀로 되돌린다.
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