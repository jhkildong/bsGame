using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class AttackBuildingBase : Building, IBuffable
{
    // Start is called before the first frame update

    public enum AtkType
    {
        Meele,
        Projectile,
        Area
    }
    [SerializeField] protected AtkType atkType;

    [SerializeField]protected GameObject target;
    protected List<Transform> detectedObj = new List<Transform>();
    public bool atkDelaying;

    public LayerMask attackableLayer;

    //public GameObject atkEffect;
    public List<GameObject> effectList; //

    public Transform effectPool; //오브젝트 풀링 될 초기 위치. 

    public UnityEvent EffectPoolEvent;

    public UnityEvent AtkEvent; //AtkDelay 에서 Invoke

    protected Vector3 relativeDir; // 투사체를 발사할 방향벡터

    public Buff getBuff { get; set; } = new Buff();
    

    /*
    protected short _atkPower;// 공격가능한 건물의 공격력
    public float _atkDelay;  // 건물의 공격 생성 딜레이
    protected float _hitDelay; // 건물 공격의 타격 간격 (지속 공격의 경우)
    protected float _atkDuration; // 건물 공격의 지속 시간( 장판 공격의 경우)
    protected float _atkRadius; // 건물의 공격 반지름 (좌표 범위형 공격)
    protected float _atkProjectileSize; //건물의 투사체 사이즈 (투사체형 공격)
    protected float _atkProjectileSpeed; // 건물 투사체 속도
    protected float _atkProjectileRange; // 건물 투사체 사거리
    protected bool _atkCanPen; //관통가능한 공격인가?
    protected int _atkPenCount; //관통가능한 물체수
    */
    /*0412 수정전 내용
    protected override void Start()
    {
        base.Start();
        attackableLayer = Data.attackableLayer;
        _atkPower = Data.atkPower;
        _atkDelay = Data.atkDelay;
        _hitDelay = Data.hitDelay;
        _atkDuration = Data.atkDuration;
        _atkRadius = Data.atkRadius;
        _atkProjectileSize = Data.atkProjectileSize;
        _atkProjectileSpeed = Data.atkProjectileSpeed;
        _atkProjectileRange = Data.atkProjectileRange;
        _atkCanPen = Data.atkCanPen;
        _atkPenCount = Data.atkPenCount;
        Debug.Log("attackbuilding" + _constTime);
    }
    */

    protected override void Start()
    {
        base.Start();
        
    }

    public void SetAtkStats()
    {
        //스탯에 변동이 있을시 호출될 함수.

    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (iscompletedBuilding && atkType == AtkType.Area)
        {
            SetAttackTarget(other);
        }
        else if(iscompletedBuilding && atkType == AtkType.Projectile)
        {
            SetAttackTarget(other);
        }
        else if(iscompletedBuilding && atkType == AtkType.Meele)
        {
            //SetAttackTarget(other);
        }
    }

    protected void SetAttackTarget(Collider other) //가장 먼저 범위안에 들어온 적을 타겟으로
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                detectedObj.Add((obj as Monster).transform);
                target = detectedObj[0].gameObject;
                //Debug.Log(target);

                (obj as Monster).DeadTransformAct += RemoveTarget; //Target을 새로 찾는이벤트를 등록해둔다. Target이 죽었을때 이벤트 발생.
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (iscompletedBuilding && (atkType == AtkType.Projectile || atkType == AtkType.Area) && (1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                if (detectedObj.Contains((obj as Monster).transform))
                {
                    detectedObj.Remove((obj as Monster).transform);
                    if(detectedObj.Count > 0)
                    {
                        target = detectedObj[0].gameObject; //새로운 타겟 찾기
                    }
                    else
                    {
                        target = null;
                    }
                    //Debug.Log(other.gameObject);
                }
            }
        }
    }

    void RemoveTarget(Transform tr)
    {
        foreach (Transform obj in detectedObj)
        {
            if(obj != null && obj.transform == tr)
            {
                detectedObj.Remove(obj);
                if (detectedObj.Count > 0)
                {
                    target = detectedObj[0].gameObject; //새로운 타겟 찾기
                }
                else
                {
                    target = null;
                }
                return;
            }
        }
    }

    /* 0412 수정전 내용
    protected override void Update() // 사실상 override 아님. Building에서 update로 하는게 없음
    {
        base.Update();
        AttackToTarget();
    }

    protected virtual void AttackToTarget()
    {
        if (target != null&&!atkDelaying)
        {   
            atkDelaying = true;
            StartCoroutine(AtkDelay(_atkDelay));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        Debug.Log("공격!");
        //여기를 이벤트로 호출할 함수를 추가해야됨.
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    protected override void ConstructComplete()
    {
        base.ConstructComplete();
        //InstEffects();
    }
    */

    protected void InstEffects() // 이펙트 오브젝트 풀링 (생성)
    {
        /*
        GameObject eft = Instantiate(atkEffect, effectPool);
        effectList.Add(eft);
        eft.SetActive(false);
        */

    }




    /*
    protected void OnMeeleAttack(Collider other)
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                obj.TakeDamage(_attackPower);
                //Debug.Log(obj);
            }
        }
    }
    */


    /*0412 수정전
    public short SetDmg()
    {
        return _atkPower;
    }
    public float SetAtkRadius()
    {
        return _atkRadius;
    }
    public float SetAtkProjectileSize()
    {
        return _atkProjectileSize;
    }
    public float SetAtkDelay()
    {
        return _atkDelay;
    }
    */

}
