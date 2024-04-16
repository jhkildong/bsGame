using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PointAtkEffectHit : MonoBehaviour, ISetPointStats
{

    protected enum Type
    {
        Once,
        Last
    }
    
    [SerializeField]protected Type atkType;

    ParticleSystem ps;

    public bool canHit;

    protected float hitTime = 1f; //공격 타이밍
    float progress; // 파티클 재생 진행도

    private float dmg;
    private float baseAttack;
    private float myRadius;
    private float atkDelay; // 공격 간격
    private float atkDuration; // 지속시간
    private float curDur; //현재 지속시간
    private bool atkDelaying;

    [SerializeField]protected float hitTiming; // 타격 타이밍 (0~1사이)

    public LayerMask attackableLayer;

    private void Awake()
    {
       
    }
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        //getParentBuildingAtkStats();
        canHit = true;
        curDur = 0f;
    }
    /*
    void getParentBuildingAtkStats()
    {
        
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>(); //커플링. 부모 건물의 현재 공격력을 갖는다.
        Debug.Log(buildingStat);
        Debug.Log("데미지 " + dmg);
        dmg = buildingStat.SetDmg();
        myRadius = buildingStat.SetAtkRadius();
        myProjectileSize = buildingStat.SetAtkProjectileSize();

    }
    */

    public void SetPointStats(float atk = 1, float radius = 1, float size = 1, float speed = 1,float delay = 1, float durTime = 1) // 건물의 스탯을 EffectPoolManager에 전달 -> 이펙트 생성시에 해당 Stat을 이펙트로 전달
    {
        baseAttack = atk;
        myRadius = radius;
        atkDelay = delay;
        atkDuration = durTime;
    }

    void Update()
    {

        if (atkType == Type.Once) // 1회 타격 공격인 경우
        {
            progress = ps.time / ps.main.duration;
            //if (Mathf.Approximately(progress, hitTime) && canHit)
            if ((progress > hitTiming) && canHit)
            {
                canHit = false;
                HitSphere();
            }
            if (progress >= 1f)
            {
                //gameObject.SetActive(false);
                EffectPoolManager.Instance.ReleaseObject<PointAtkEffectHit>(gameObject);
            }
        }

        else if (atkType == Type.Last) // 지속공격인 경우 ( 장판기 )
        {
             progress = ps.time / ps.main.duration;
             curDur += Time.deltaTime;
            //지속시간까지 반복해서 딜레이마다 공격.
            if (!atkDelaying && (progress > hitTiming) && curDur < atkDuration)
            {
                atkDelaying = true;
                StartCoroutine(LastAtk(atkDelay));
            }
            else if(curDur >= atkDuration) // 지속시간이 끝나면 풀로 되돌림
            {       
                atkDelaying = false;
                EffectPoolManager.Instance.ReleaseObject<PointAtkEffectHit>(gameObject);
            }
        }

    }

    IEnumerator LastAtk(float delay) //타격 시간간격을 구현하기 위한 코루틴 함수
    {
        HitSphere();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    protected void SetHitTiming(float timing)
    {
        hitTime = timing;

    }

    protected void HitSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, myRadius, attackableLayer); 
        if(colliders.Length > 0 )
        {
            //Debug.Log(colliders);
        }

        foreach (Collider collider in colliders)
        {
            IDamage target = collider.GetComponent<IDamage>();
            target.TakeDamage(baseAttack);
        }
        //범위내의 적에게 (Idamage 가 있는)
        //데미지 전달
    }

}
