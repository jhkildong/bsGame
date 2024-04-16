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
        _atkDelay = PData.atkDelay;
        _atkProjectileSize = PData.atkProjectileSize;
        _atkProjectileSpeed = PData.atkProjectileSpeed;
        _atkProjectileRange = PData.atkProjectileRange;
        _atkCanPen = PData.atkCanPen;
        _atkPenCount = PData.atkPenCount;
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
            StartCoroutine(AtkDelay(_atkDelay));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        Debug.Log("����!");

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
