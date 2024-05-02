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
    [SerializeField] protected float _finalAs; // 최종공속
    [SerializeField] protected float _finalRadius; // 최종 공격범위
    [SerializeField] protected float _atkPower;// 건물의 기본 공격력
    [SerializeField] protected float _atkSpeed;  // 건물의 공격 생성 딜레이
    [SerializeField] protected float _hitDelay; // 건물 공격의 타격 간격 (지속 공격의 경우)
    [SerializeField] protected float _atkDuration; // 건물 공격의 지속 시간( 장판 공격의 경우)
    [SerializeField] protected float _atkRadius; // 건물 공격의 반지름 (좌표 범위형 공격)

    [SerializeField] protected float _additionalAtk; // 추가공격력
    //[SerializeField] private LayerMask _attackableLayer;
    [SerializeField] protected int _atkId;
    public PointAtkEffectHit atkEffect;
    protected override void Start()
    {
        base.Start();
        attackableLayer = AData.attackableLayer;
        _atkPower = AData.atkPower;
        _atkSpeed = AData.atkSpeed;
        _hitDelay = AData.hitDelay;
        _atkDuration = AData.atkDuration;
        _atkRadius = AData.atkRadius;
        _atkId = atkEffect.ID;

        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff));
        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (기본공격속도 * (1 + %공격속도합산))
        _finalRadius = _atkRadius + (_atkRadius * getBuff.rangeBuff); // 기본 범위 + (기본범위 * %범위합산)
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

            StartCoroutine(AtkDelay(_atkSpeed));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        /*
        Debug.Log("공격!");
        //여기를 이벤트로 호출할 함수를 추가해야됨.
        float sumBuff = 0; // 버프의 합을 계산할 변수
        //버프 가산 계산하여 최종데미지 구하기.
        foreach (float buffs in getBuff.atkBuffDict.Values) //공버프 리스트의 값들을 모두 합연산
        {
            sumBuff += buffs;
            Debug.Log("버프합산" + sumBuff);
        }
        getBuff.atkBuff = sumBuff;
        */
        _finalDmg = Mathf.Round((float)_atkPower * (1 + getBuff.atkBuff) + _additionalAtk); // 기본공격력 * (1 + (%공격력합산)) + 추가데미지

        _finalAs = 1 / (_atkSpeed * (1 + getBuff.asBuff)); // 1/ (기본공격속도 * (1 + %공격속도합산))

        _finalRadius = _atkRadius + (_atkRadius * getBuff.rangeBuff); // 기본 범위 + (기본범위 * %범위합산)
        Debug.Log(_finalDmg);
        Debug.Log("최종 공속" + _finalAs);
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(_finalAs);
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
