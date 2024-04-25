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

    public GameObject myAtkCollider; //���� ���� Collider

    [SerializeField] protected float _finalDmg; // ����������
    [SerializeField] protected float _finalRadius; // ���� ���ݹ���


    [SerializeField] private float _atkPower;
    [SerializeField] private float _hitDelay; // �ǹ������� Ÿ�� ����
    [SerializeField] private float _atkRadius; // ������ ����
    //[SerializeField] private LayerMask _attackableLayer;


    protected override void Start()
    {
        base.Start();
        attackableLayer = MData.attackableLayer;
        _atkPower = MData.atkPower;
        _hitDelay = MData.hitDelay;
        _atkRadius = MData.atkRadius;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalRadius = _atkRadius + (_atkRadius * getBuff.rangeBuff); // �⺻ ���� + (�⺻���� * %�����ջ�)
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
        return _finalDmg;
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
