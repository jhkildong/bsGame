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


    [SerializeField] protected float _finalRadius; // ���� ���ݹ���
    [SerializeField] protected float _finalDmg; // ���� ���ݷ�

    [SerializeField] private float _atkPower;
    [SerializeField] private float _hitDelay; // �ǹ������� Ÿ�� ����
    [SerializeField] private float _atkRadius; // ������ ����
    //[SerializeField] private LayerMask _attackableLayer;
    [SerializeField] private float _additionalAtk; // ������ ����

    protected override void Start()
    {
        base.Start();
        attackableLayer = MData.attackableLayer;
        _atkPower = MData.atkPower;
        _hitDelay = MData.hitDelay;
        _atkRadius = MData.atkRadius;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff) + _additionalAtk); // �⺻���ݷ� * (1 + (%���ݷ��ջ�)) + �߰�������
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
