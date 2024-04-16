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

    [SerializeField] public short _atkPower;
    [SerializeField] public float _atkDelay;
    [SerializeField] public float _atkProjectileSize;
    [SerializeField] public float _atkProjectileSpeed; // 건물 투사체 속도
    [SerializeField] public float _atkProjectileRange; // 건물 투사체 사거리
    [SerializeField] public bool _atkCanPen; //관통가능한 공격인가?
    [SerializeField] public int _atkPenCount; //관통가능한 물체수
    //[SerializeField] private LayerMask _attackableLayer;
    protected override void Start()
    {
        base.Start();
        attackableLayer = PData.attackableLayer;
        _atkPower = PData.atkPower;
        _atkDelay = PData.atkDelay;
        _atkProjectileSize = PData.atkProjectileSize;
        _atkProjectileSpeed = PData.atkProjectileSpeed;
        _atkProjectileRange = PData.atkProjectileRange;
        _atkCanPen = PData.atkCanPen;
        _atkPenCount = PData.atkPenCount;
    }


    protected override void Update() // 사실상 override 아님. Building에서 update로 하는게 없음
    {
        base.Update();
        AttackToTarget();
    }

    protected virtual void AttackToTarget()
    {
        if (target != null && !atkDelaying)
        {
            atkDelaying = true;
            StartCoroutine(AtkDelay(_atkDelay));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        Debug.Log("공격!");

        AtkEvent?.Invoke();
        yield return new WaitForSeconds(delay);
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
