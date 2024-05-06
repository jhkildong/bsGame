using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityAction ChangeBuffAct;

    Dictionary<string, float> buffDict;
    private float buffAmount;
    private bool isDirty = true;

    public BuffDict()
    {
        buffDict = new Dictionary<string, float>();
        isDirty = true;
    }

    public float this[string key]
    {
        get
        {
            if (buffDict.ContainsKey(key))
            {
                return buffDict[key];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            isDirty = true;
            if (buffDict.ContainsKey(key))
            {
                buffDict[key] = value;
                ChangeBuffAct?.Invoke();
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public void Add(string key, float value)
    {
        if (buffDict.ContainsKey(key))
            return;
        buffDict.Add(key, value);
        ChangeBuffAct?.Invoke();
        isDirty = true;
    }
    public void Remove(string key)
    {
        if (!buffDict.ContainsKey(key))
            return;
        buffDict.Remove(key);
        ChangeBuffAct?.Invoke();
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
