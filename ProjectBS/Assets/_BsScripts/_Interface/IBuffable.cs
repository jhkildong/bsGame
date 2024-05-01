using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffable
{
    Buff getBuff { get; set; }
}

public class Buff
{
    public BuffDict atkBuffDict = new BuffDict();
    public BuffDict hpBuffDict = new BuffDict();
    public BuffDict asBuffDict = new BuffDict();
    public BuffDict msBuffDict = new BuffDict();
    public BuffDict rangeBuffDict = new BuffDict();

    //public Dictionary<string, float> atkBuffDict = new Dictionary<string, float>();
    public float atkBuff
    {
        get
        {
            return atkBuffDict.CalcBuff();
        }
    }

    //public Dictionary<string, float> hpBuffDict = new Dictionary<string, float>();
    public float hpBuff // ü�� ����
    {
        get
        {
            return hpBuffDict.CalcBuff();
        }
    }

    //public Dictionary<string, float> asBuffDict = new Dictionary<string, float>();
    public float asBuff
    {
        get
        {
            return asBuffDict.CalcBuff();
        }
    }

    //public Dictionary<string, float> msBuffDict = new Dictionary<string, float>();
    public float msBuff
    {
        get
        {
            return msBuffDict.CalcBuff();
        }
    }

    //public Dictionary<string, float> rangeBuffDict = new Dictionary<string, float>();
    public float rangeBuff
    {
        get
        {
            return rangeBuffDict.CalcBuff();
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
    /*
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
    */
}

//���� �ٲ� ��츸 ����ϵ��� ���� ���� ��ųʸ� Ŭ���� -��ȯ �߰�
public class BuffDict
{
    Dictionary<string, float> buffDict;
    private float buffAmount;
    private bool isDirty = true;

    public BuffDict()
    {
        buffDict = new Dictionary<string, float>();
        isDirty = true;
    }

    public void Add(string key, float value)
    {
        if (buffDict.ContainsKey(key))
        {
            Debug.Log("Key already exists");
            //throw new System.Exception("Key already exists");
        }
        else
        {
            buffDict.Add(key, value);
        }
        isDirty = true;
    }
    public void Remove(string key)
    {
        if(buffDict.ContainsKey(key))
        {
            buffDict.Remove(key);
        }
        else
        {
            throw new System.Exception("Key not found");
        }
        isDirty = true;
    }

    public bool ContainsKey(string key)
    {
        return buffDict.ContainsKey(key);
    }

    public float CalcBuff()
    {
        if (!isDirty)
            return buffAmount;
        isDirty = false;
        
        float sumBuff = 0;
        foreach (float buffs in buffDict.Values)
        {
            sumBuff += buffs;
            Debug.Log("�����ջ�" + sumBuff);
        }
        buffAmount = sumBuff;
        return buffAmount;
    }
}
