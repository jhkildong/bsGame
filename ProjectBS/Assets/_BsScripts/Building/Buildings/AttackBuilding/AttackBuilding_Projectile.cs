using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackBuilding_Projectile : AttackBuildingBase
{
    [SerializeField] private ProjectileAttackBuildingData AreaBuildingData;
    public ProjectileAttackBuildingData PData
    {
        get { return AreaBuildingData; }
        set { AreaBuildingData = value; }
    }
    [SerializeField] protected float _finalDmg; // 최종데미지
    [SerializeField] protected float _finalAs; // 최종공속
    [SerializeField] protected float _finalSize; // 최종 투사체 크기

    [SerializeField] public float _atkPower;
    [SerializeField] public float _atkSpeed;
    [SerializeField] public float _atkProjectileSize; // 투사체 크기
    [SerializeField] public float _atkProjectileSpeed; // 건물 투사체 속도
    [SerializeField] public float _atkProjectileRange; // 건물 투사체 사거리
    [SerializeField] public bool _atkCanPen; //관통가능한 공격인가?
    [SerializeField] public int _atkPenCount; //관통가능한 물체수

    [SerializeField] protected int _atkId;
    public ProjectileEffectHit atkEffect;
    protected override void Start()
    {
        base.Start();
        attackableLayer = PData.attackableLayer;
        _atkPower = PData.atkPower;
        _atkSpeed = PData.atkSpeed;
        _atkProjectileSize = PData.atkProjectileSize;
        _atkProjectileSpeed = PData.atkProjectileSpeed;
        _atkProjectileRange = PData.atkProjectileRange;
        _atkCanPen = PData.atkCanPen;
        _atkPenCount = PData.atkPenCount;
        _atkId = atkEffect.ID;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (기본공격속도 * (1 + %공격속도합산))
        _finalSize = _atkProjectileSize + (_atkProjectileSize * getBuff.rangeBuff); 

    }


    protected override void Update() 
    {
        base.Update();
        AttackToTarget();
    }

    protected virtual void AttackToTarget()
    {
        if (target != null && !atkDelaying)
        {
            atkDelaying = true;
            StartCoroutine(AtkDelay(_atkSpeed));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        Debug.Log("공격!");
        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (기본공격속도 * (1 + %공격속도합산))
        _finalSize = _atkProjectileSize + (_atkProjectileSize * getBuff.rangeBuff);

        AtkEvent?.Invoke(); // 공격 로직 실행 ( 타겟으로 발사 )
        yield return new WaitForSeconds(_finalAs);
        atkDelaying = false;
    }


    /*
    protected override void ConstructComplete()
    {
        base.ConstructComplete();
        //InstEffects();
    }
    */
}
