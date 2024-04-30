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
    public float hpBuff // ü�� ����
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
    public int AdditionalAtk // �߰� ���ݷ� ����
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
        //����
        float sumBuff = 0; // ������ ���� ����� ����
        //���� ���� ����Ͽ� ���������� ���ϱ�.
        foreach (float buffs in buffDict.Values) //������ ����Ʈ�� ������ ��� �տ���
        {
            sumBuff += buffs;
            Debug.Log("�����ջ�" + sumBuff);
        }
        return sumBuff;
    }

}
