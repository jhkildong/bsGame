using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IBuffable
    {
        Buff getBuff { get; set; }
    }

    public class Buff
    {
    /* 0421 ������
        public Dictionary<string, float> atkBuffDict = new Dictionary<string,float>();
        public float atkBuff = 1.0f;//���ݷ� ����

        public Dictionary<string, float> hpBuffDict = new Dictionary<string, float>();
        public float hpBuff = 1.0f;//ü�� ����

        public Dictionary<string, float> asBuffDict = new Dictionary<string, float>();
        public float asBuff = 1.0f; //���� ����
    
        public Dictionary<string, float> msBuffDict = new Dictionary<string, float>();
        public float msBuff = 1.0f; //�̼� ����
    */
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
