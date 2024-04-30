using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IBuffable
    {
        Buff getBuff { get; set; }
    }

    public class Buff
    {
    public Dictionary<string, float> atkBuffDict = new Dictionary<string, float>();
    public float atkBuff
    {
        get
        {
            return CalcBuff(atkBuffDict);
        }
    }

    public Dictionary<string, float> hpBuffDict = new Dictionary<string, float>();
    public float hpBuff // 체력 버프
    {
        get
        {
            return CalcBuff(hpBuffDict);
        }
    }

    public Dictionary<string, float> asBuffDict = new Dictionary<string, float>();
    public float asBuff
    {
        get
        {
            return CalcBuff(asBuffDict);
        }
    }

    public Dictionary<string, float> msBuffDict = new Dictionary<string, float>();
    public float msBuff
    {
        get
        {
            return CalcBuff(msBuffDict);
        }
    }

    public Dictionary<string, float> rangeBuffDict = new Dictionary<string, float>();
    public float rangeBuff
    {
        get
        {
            return CalcBuff(rangeBuffDict);
        }
    }
    /*
    public int AdditionalAtk // 추가 공격력 가산
    {
        get => _additionalAtk;
        set
        {
            _additionalAtk += value;
        }
    }

    private int _additionalAtk;
    */

    public float CalcBuff(Dictionary<string, float> buffDict)
    {
        //수식
        float sumBuff = 0; // 버프의 합을 계산할 변수
        //버프 가산 계산하여 최종데미지 구하기.
        foreach (float buffs in buffDict.Values) //공버프 리스트의 값들을 모두 합연산
        {
            sumBuff += buffs;
            Debug.Log("버프합산" + sumBuff);
        }
        return sumBuff;
    }

}
