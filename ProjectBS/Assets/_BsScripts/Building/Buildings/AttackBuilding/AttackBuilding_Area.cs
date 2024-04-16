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

    [SerializeField] protected float _finalDmg; // 최종데미지
    [SerializeField] protected float _atkPower;// 공격가능한 건물의 공격력
    [SerializeField] protected float _atkDelay;  // 건물의 공격 생성 딜레이
    [SerializeField] protected float _hitDelay; // 건물 공격의 타격 간격 (지속 공격의 경우)
    [SerializeField] protected float _atkDuration; // 건물 공격의 지속 시간( 장판 공격의 경우)
    [SerializeField] protected float _atkRadius; // 건물의 공격 반지름 (좌표 범위형 공격)
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
        //여기를 이벤트로 호출할 함수를 추가해야됨.

        float bd = getBuff.atkBuff;

        //버프 가산 계산하여 최종데미지 구하기.
        _finalDmg = Mathf.Round((float)_atkPower * bd);
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    /*0415 수정전 문제없으면 삭제
    protected override void ConstructComplete()
    {
        base.ConstructComplete();
        //InstEffects();
    }
    */



}
