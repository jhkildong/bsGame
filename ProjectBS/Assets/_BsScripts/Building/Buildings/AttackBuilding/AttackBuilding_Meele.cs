using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AttackBuilding_Meele : AttackBuildingBase
{
    [SerializeField] private MeeleAttackBuildingData MeeleBuildingData;
    public MeeleAttackBuildingData MData
    {
        get { return MeeleBuildingData; }
        set { MeeleBuildingData = value; }
    }

    public GameObject myAtkCollider; //공격 범위 Collider


    [SerializeField] protected float _finalRadius; // 최종 공격범위
    [SerializeField] protected float _finalDmg; // 최종 공격력

    [SerializeField] private float _atkPower;
    [SerializeField] private float _hitDelay; // 건물공격의 타격 간격
    [SerializeField] private float _atkRadius; // 공격의 범위
    //[SerializeField] private LayerMask _attackableLayer;
    [SerializeField] private float _additionalAtk; // 공격의 범위

    protected override void Start()
    {
        base.Start();
        attackableLayer = MData.attackableLayer;
        _atkPower = MData.atkPower;
        _hitDelay = MData.hitDelay;
        _atkRadius = MData.atkRadius;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff) + _additionalAtk); // 기본공격력 * (1 + (%공격력합산)) + 추가데미지
        _finalRadius = _atkRadius + (_atkRadius * getBuff.rangeBuff); // 기본 범위 + (기본범위 * %범위합산)
    }
    protected void MeeleAttack()
    {
        //SetActiveEffects();
        //MeeleAtk();
    }

    protected override void ConstructComplete() 
    { 
        base.ConstructComplete();
        myAtkCollider.SetActive(true);
    }


    public float SetDmg()
    {
        return _atkPower;
    }
    public float SetAtkRadius()
    {
        return _atkRadius;
    }
    public float SetHitDelay()
    {
        return _hitDelay;
    }
    public LayerMask SetAttackableMask()
    {
        return attackableLayer;
    }
}
