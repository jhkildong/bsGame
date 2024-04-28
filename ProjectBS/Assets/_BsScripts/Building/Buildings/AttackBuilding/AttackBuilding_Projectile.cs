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
    [SerializeField] protected float _finalDmg; // ����������
    [SerializeField] protected float _finalAs; // ��������
    [SerializeField] protected float _finalSize; // ���� ����ü ũ��

    [SerializeField] public float _atkPower;
    [SerializeField] public float _atkSpeed;
    [SerializeField] public float _atkProjectileSize; // ����ü ũ��
    [SerializeField] public float _atkProjectileSpeed; // �ǹ� ����ü �ӵ�
    [SerializeField] public float _atkProjectileRange; // �ǹ� ����ü ��Ÿ�
    [SerializeField] public bool _atkCanPen; //���밡���� �����ΰ�?
    [SerializeField] public int _atkPenCount; //���밡���� ��ü��
    //[SerializeField] private LayerMask _attackableLayer;
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

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (�⺻���ݼӵ� * (1 + %���ݼӵ��ջ�))
        _finalSize = _atkProjectileSize + (_atkProjectileSize * getBuff.rangeBuff); 

    }


    protected override void Update() // ��ǻ� override �ƴ�. Building���� update�� �ϴ°� ����
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
        Debug.Log("����!");
        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (�⺻���ݼӵ� * (1 + %���ݼӵ��ջ�))
        _finalSize = _atkProjectileSize + (_atkProjectileSize * getBuff.rangeBuff);

        AtkEvent?.Invoke();
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
