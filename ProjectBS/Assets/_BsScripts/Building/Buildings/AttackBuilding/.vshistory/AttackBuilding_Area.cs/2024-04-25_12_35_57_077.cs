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
    [SerializeField] protected float _finalAs; // ��������
    [SerializeField] protected float _atkPower;// ���ݰ����� �ǹ��� ���ݷ�
    [SerializeField] protected float _atkSpeed;  // �ǹ��� ���� ���� ������
    [SerializeField] protected float _hitDelay; // �ǹ� ������ Ÿ�� ���� (���� ������ ���)
    [SerializeField] protected float _atkDuration; // �ǹ� ������ ���� �ð�( ���� ������ ���)
    [SerializeField] protected float _atkRadius; // �ǹ��� ���� ������ (��ǥ ������ ����)


    //[SerializeField] private LayerMask _attackableLayer;

    protected override void Start()
    {
        base.Start();
        attackableLayer = AData.attackableLayer;
        _atkPower = AData.atkPower;
        _atkSpeed = AData.atkSpeed;
        _hitDelay = AData.hitDelay;
        _atkDuration = AData.atkDuration;
        _atkRadius = AData.atkRadius;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (�⺻���ݼӵ� * (1 + %���ݼӵ��ջ�))
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

            StartCoroutine(AtkDelay(_atkSpeed));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        /*
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
        */
        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff) + _additonalAtk); // �⺻���ݷ� * (1 + (%���ݷ��ջ�)) + �߰�������

        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (�⺻���ݼӵ� * (1 + %���ݼӵ��ջ�))
        Debug.Log(_finalDmg);
        Debug.Log("���� ����" + _finalAs);
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(_finalAs);
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
