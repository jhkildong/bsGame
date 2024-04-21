using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuilding_Area : AttackBuildingBase
{
    [SerializeField]private AreaAttackBuildingData AreaBuildingData;
    public AreaAttackBuildingData AData
    {
        get { return AreaBuildingData; }
        set { AreaBuildingData = value; }
    }

    [SerializeField] protected float _finalDmg; // ����������
    [SerializeField] protected float _atkPower;// ���ݰ����� �ǹ��� ���ݷ�
    [SerializeField] protected float _atkDelay;  // �ǹ��� ���� ���� ������
    [SerializeField] protected float _hitDelay; // �ǹ� ������ Ÿ�� ���� (���� ������ ���)
    [SerializeField] protected float _atkDuration; // �ǹ� ������ ���� �ð�( ���� ������ ���)
    [SerializeField] protected float _atkRadius; // �ǹ��� ���� ������ (��ǥ ������ ����)
    //[SerializeField] private LayerMask _attackableLayer;

    protected override void Start()
    {
        base.Start();
        attackableLayer = AData.attackableLayer;
        _atkPower = AData.atkPower;
        _atkDelay = AData.atkDelay;
        _hitDelay = AData.hitDelay;
        _atkDuration = AData.atkDuration;
        _atkRadius = AData.atkRadius;
    }


        protected void PointAttack()
    {
        //SetActivePointAtkEffect();
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
        //���⸦ �̺�Ʈ�� ȣ���� �Լ��� �߰��ؾߵ�.
        float sumBuff = 0; // ������ ���� ����� ����
        //���� ���� ����Ͽ� ���������� ���ϱ�.
        foreach (float buffs in getBuff.atkBuffDict.Values) //������ ����Ʈ�� ������ ��� �տ���
        {
            sumBuff += buffs;
            Debug.Log("�����ջ�" + sumBuff);
        }
        getBuff.atkBuff = sumBuff;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        Debug.Log(_finalDmg);
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    /*0415 ������ ���������� ����
    protected override void ConstructComplete()
    {
        base.ConstructComplete();
        //InstEffects();
    }
    */



}
