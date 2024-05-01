using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffController
{
    BuffController buffController { get; set; }
}

public class BuffController
{
    public MonoBehaviour This;
    public string BuffName;
    public float BuffAmount;
    public bool HasDuration;
    public float Duration;
    public bool CanStack;

    public BuffController(MonoBehaviour mono, string name, float amount, bool hasDuration, float dur, bool stack)
    {
        This = mono;
        BuffName = name;
        BuffAmount = amount;
        HasDuration = hasDuration;
        Duration = dur;
        CanStack = stack;
    }


    //public void StartBuff(IBuffable buffable, Dictionary<string,float> buffTypeDict) // ontriggerenter��
    public void StartBuff(IBuffable buffable, BuffDict buffTypeDict)
    {
        Debug.Log(buffable);
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // ������ ���� ��������
            if (buff == null)
            {
                buff = new Buff(); // ������ ������ ���� ����
            }
            // ���� �� ����
            //buff.atkBuffList.Add(buffAmount);
            if (CanStack) // ��ø�� ������ ���
            {
                if (HasDuration) // ���ӽð��� �ִ� ������ ���
                {
                    while (true)
                    {
                        if (buffTypeDict.ContainsKey(BuffName)) // buff.atkBuffDict -> buffType
                        {
                            BuffName += "1";
                        }
                        else
                        {
                            buffTypeDict.Add(BuffName, BuffAmount); //���� ����
                            This.StartCoroutine(BuffTime(buffTypeDict, Duration)); //���� ���ӽð� �ڷ�ƾ // ������ (buff, duration)
                            break;
                        }
                    }
                }
                else // ���ӽð��� ���� ������ ���
                {
                    while (true)
                    {
                        if (buffTypeDict.ContainsKey(BuffName))
                        {
                            BuffName += "1";
                        }
                        else
                        {
                            buffTypeDict.Add(BuffName, BuffAmount); //���� ����
                            break;
                        }
                    }
                }
            }
            else // ��ø�� �Ұ����� ���
            {
                if (HasDuration) // ���ӽð��� �ִ� ������ ���
                {
                    buffTypeDict.Remove(BuffName);
                    buffTypeDict.Add(BuffName, BuffAmount); //���� �߰�
                    This.StartCoroutine(BuffTime(buffTypeDict, Duration)); //���� ���ӽð� �ڷ�ƾ

                }
                else // ���ӽð��� ���� ������ ���
                {
                    buffTypeDict.Remove(BuffName);
                    buffTypeDict.Add(BuffName, BuffAmount); //���� �߰�
                }
            }

            //buff.atkBuffDict.Add(buffName, buffAmount); 
            buffable.getBuff = buff; // ���� ����
            Debug.Log("������" + BuffName);
        }
    }

    //public void RemoveBuff(IBuffable buffable, Dictionary<string,float> buffTypeDict)
    public void RemoveBuff(IBuffable buffable, BuffDict buffTypeDict)
    {
        //targets ����Ʈ�� �ش� ������Ʈ�� �ִ��� üũ
        Debug.Log(buffable);
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // ������ ���� ��������
            if (buff == null)
            {
                //buff = new Buff(); // ������ ������ ���� ����
                return;
            }
            // ���� �� ����
            //buff.atkBuffList.Add(buffAmount);
            buffTypeDict.Remove(BuffName); // buff.atkBuffDict -> buffTypeDict

            buffable.getBuff = buff; // ���� ����
            Debug.Log("����������");
        }
    }



    //IEnumerator BuffTime(Dictionary<string, float> buffTypeDict, float dur)
    IEnumerator BuffTime(BuffDict buffTypeDict, float dur)
    {
        yield return new WaitForSeconds(dur);
        buffTypeDict.Remove(BuffName); // ���ӽð��� ������ ���� ����
    }
}